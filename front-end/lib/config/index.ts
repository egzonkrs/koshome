/**
 * Application Configuration
 * Centralized configuration management with type safety.
 */

export const config = {
  api: {
    baseUrl: process.env.NEXT_PUBLIC_API_URL || "https://localhost:5000",
    version: process.env.NEXT_PUBLIC_API_VERSION || "v1",
    get url() {
      return `${this.baseUrl}/api/${this.version}`
    },
  },

  features: {
    useMock: process.env.NEXT_PUBLIC_USE_MOCK === "true",
    enableDevtools: process.env.NEXT_PUBLIC_ENABLE_DEVTOOLS === "true",
  },

  auth: {
    tokenKey: "koshome_access_token",
    refreshTokenKey: "koshome_refresh_token",
    userKey: "koshome_user",
  },
} as const

export type Config = typeof config
