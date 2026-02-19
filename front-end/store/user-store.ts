import { create } from "zustand"
import { persist } from "zustand/middleware"
import { authProvider } from "@/lib/api/providers"
import type { AuthResponse, CurrentUser } from "@/lib/api/types"

export type UserPlan = "free" | "standard" | "premium"

export interface User {
  id: string
  name: string
  email: string
  plan: UserPlan
  credits: number
}

interface UserState {
  user: User | null
  isAuthenticated: boolean

  // Actions
  login: (email: string, password: string) => Promise<boolean>
  signup: (name: string, email: string, password: string) => Promise<boolean>
  logout: () => void
  useCredit: () => boolean
  upgradePlan: (plan: UserPlan) => void
  setCredits: (credits: number) => void
}

const getCreditsForPlan = (plan: UserPlan): number => {
  switch (plan) {
    case "free":
      return 0
    case "standard":
      return 10
    case "premium":
      return 999 // Effectively unlimited
    default:
      return 0
  }
}

const mapCurrentUser = (currentUser: CurrentUser, plan: UserPlan = "free"): User => {
  const name = `${currentUser.firstName} ${currentUser.lastName}`.trim()

  return {
    id: currentUser.userId,
    name: name || currentUser.email.split("@")[0],
    email: currentUser.email,
    plan,
    credits: getCreditsForPlan(plan),
  }
}

const mapAuthResponse = (response: AuthResponse, email: string): User => {
  const name = `${response.firstName || ""} ${response.lastName || ""}`.trim()

  return {
    id: response.userId || `user-${Date.now()}`,
    name: name || email.split("@")[0],
    email,
    plan: "free",
    credits: getCreditsForPlan("free"),
  }
}

export const useUserStore = create<UserState>()(
  persist(
    (set, get) => ({
      user: null,
      isAuthenticated: false,

      login: async (email: string, password: string) => {
        try {
          const response = await authProvider.login({ email, password })
          let user: User

          try {
            const currentUser = await authProvider.getCurrentUser()
            user = mapCurrentUser(currentUser, "standard")
          } catch {
            user = mapAuthResponse(response, email)
          }

          set({
            user,
            isAuthenticated: true,
          })
          return true
        } catch (error) {
          console.error("Login failed:", error)
          return false
        }
      },

      signup: async (name: string, email: string, password: string) => {
        try {
          const [firstName, ...rest] = name.trim().split(" ")
          const lastName = rest.join(" ") || "User"
          await authProvider.register({ firstName, lastName, email, password })

          const user = mapCurrentUser(
            {
              userId: `user-${Date.now()}`,
              firstName,
              lastName,
              email,
              roles: ["user"],
            },
            "free",
          )

          set({
            user,
            isAuthenticated: true,
          })
          return true
        } catch (error) {
          console.error("Signup failed:", error)
          return false
        }
      },

      logout: () => {
        authProvider.logout().catch((error) => console.error("Logout failed:", error))
        set({ user: null, isAuthenticated: false })
      },

      useCredit: () => {
        const { user } = get()
        if (!user) return false

        if (user.plan === "premium") return true // Unlimited for premium
        if (user.credits <= 0) return false

        set({
          user: {
            ...user,
            credits: user.credits - 1,
          },
        })
        return true
      },

      upgradePlan: (plan: UserPlan) => {
        const { user } = get()
        if (!user) return

        set({
          user: {
            ...user,
            plan,
            credits: getCreditsForPlan(plan),
          },
        })
      },

      setCredits: (credits: number) => {
        const { user } = get()
        if (!user) return

        set({
          user: {
            ...user,
            credits,
          },
        })
      },
    }),
    {
      name: "koshome-user",
    },
  ),
)
