"use client"

import { useState, useEffect } from "react"
import { useForm } from "react-hook-form"
import type { ZodType } from "zod"
import { Eye, EyeOff, Mail, Lock, User, AlertCircle, CheckCircle } from "lucide-react"
import { signupSchema, type SignupFormValues } from "@/lib/validations/auth"
import { useUserStore } from "@/store/user-store"
import { useToast } from "@/hooks/use-toast"

import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { Alert, AlertDescription } from "@/components/ui/alert"
import { cn } from "@/lib/utils"

interface SignupModalProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

function safeZodResolver<T extends ZodType>(schema: T) {
  return async (values: Record<string, unknown>) => {
    const result = schema.safeParse(values)
    if (result.success) {
      return { values: result.data, errors: {} }
    }
    const errors: Record<string, { type: string; message: string }> = {}
    for (const error of result.error.errors) {
      const path = error.path.join(".")
      if (!errors[path]) {
        errors[path] = { type: error.code, message: error.message }
      }
    }
    return { values: {}, errors }
  }
}

function PasswordStrength({ password }: { password: string }) {
  const requirements = [
    { label: "At least 8 characters", met: password.length >= 8 },
    { label: "One uppercase letter", met: /[A-Z]/.test(password) },
    { label: "One lowercase letter", met: /[a-z]/.test(password) },
    { label: "One number", met: /[0-9]/.test(password) },
    { label: "One special character", met: /[!@#$%^&*(),.?":{}|<>]/.test(password) },
  ]

  const strength = requirements.filter((r) => r.met).length
  const strengthLabel = strength <= 2 ? "Weak" : strength <= 4 ? "Medium" : "Strong"
  const strengthColor = strength <= 2 ? "bg-destructive" : strength <= 4 ? "bg-yellow-500" : "bg-primary"

  if (!password) return null

  return (
    <div className="mt-2 space-y-2">
      <div className="flex items-center gap-2">
        <div className="flex-1 h-1 bg-muted rounded-full overflow-hidden">
          <div className={cn("h-full transition-all", strengthColor)} style={{ width: `${(strength / 5) * 100}%` }} />
        </div>
        <span
          className={cn(
            "text-xs font-medium",
            strength <= 2 ? "text-destructive" : strength <= 4 ? "text-yellow-600" : "text-primary",
          )}
        >
          {strengthLabel}
        </span>
      </div>
      <ul className="grid grid-cols-2 gap-1">
        {requirements.map((req) => (
          <li key={req.label} className="flex items-center gap-1 text-xs">
            {req.met ? (
              <CheckCircle className="h-3 w-3 text-primary" />
            ) : (
              <div className="h-3 w-3 rounded-full border border-muted-foreground/30" />
            )}
            <span className={cn(req.met ? "text-foreground" : "text-muted-foreground")}>{req.label}</span>
          </li>
        ))}
      </ul>
    </div>
  )
}

export function SignupModal({ open, onOpenChange }: SignupModalProps) {
  const [showPassword, setShowPassword] = useState(false)
  const [showConfirmPassword, setShowConfirmPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [serverError, setServerError] = useState<string | null>(null)

  const { signup } = useUserStore()
  const { toast } = useToast()

  const form = useForm<SignupFormValues>({
    resolver: safeZodResolver(signupSchema),
    defaultValues: {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
      terms: false,
    },
    mode: "onSubmit",
    reValidateMode: "onChange",
  })

  // Watch password for strength indicator
  const password = form.watch("password")

  // Listen for custom event to open signup modal
  useEffect(() => {
    const handleOpenSignupModal = () => {
      onOpenChange(true)
    }

    document.addEventListener("open-signup-modal", handleOpenSignupModal)

    return () => {
      document.removeEventListener("open-signup-modal", handleOpenSignupModal)
    }
  }, [onOpenChange])

  async function onSubmit(data: SignupFormValues) {
    setIsLoading(true)
    setServerError(null)

    try {
      const success = await signup(data.name, data.email, data.password)

      if (success) {
        toast({
          title: "Account created!",
          description: "Welcome to KosHome. You can now manage your listings.",
        })
        onOpenChange(false)
        form.reset()
      } else {
        setServerError("An error occurred during registration. Please try again.")
      }
    } catch (error) {
      console.error("Signup failed:", error)
      setServerError("An error occurred during registration. Please try again.")
    } finally {
      setIsLoading(false)
    }
  }

  const handleOpenChange = (isOpen: boolean) => {
    if (!isOpen) {
      form.reset()
      setServerError(null)
    }
    onOpenChange(isOpen)
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent className="sm:max-w-[425px] bg-[#eeefe9] dark:bg-background max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-center">Create an account</DialogTitle>
          <DialogDescription className="text-center">Enter your details to create your account</DialogDescription>
        </DialogHeader>

        {serverError && (
          <Alert variant="destructive" className="mt-2">
            <AlertCircle className="h-4 w-4" />
            <AlertDescription>{serverError}</AlertDescription>
          </Alert>
        )}

        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 pt-4">
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <FormItem className="form-field h-[90px] mb-2">
                  <FormLabel className="text-foreground">Full Name</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <User className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                      <Input placeholder="John Doe" className="pl-10" autoComplete="name" {...field} />
                    </div>
                  </FormControl>
                  <div className="h-[20px]">
                    <FormMessage className="text-xs" />
                  </div>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem className="form-field h-[90px] mb-2">
                  <FormLabel className="text-foreground">Email</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Mail className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                      <Input placeholder="name@example.com" className="pl-10" autoComplete="email" {...field} />
                    </div>
                  </FormControl>
                  <div className="h-[20px]">
                    <FormMessage className="text-xs" />
                  </div>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem className="mb-2">
                  <FormLabel className="text-foreground">Password</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                      <Input
                        type={showPassword ? "text" : "password"}
                        placeholder="••••••••"
                        className="pl-10"
                        autoComplete="new-password"
                        {...field}
                      />
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="absolute right-0 top-0 h-full px-3"
                        onClick={() => setShowPassword(!showPassword)}
                      >
                        {showPassword ? (
                          <EyeOff className="h-4 w-4 text-muted-foreground" />
                        ) : (
                          <Eye className="h-4 w-4 text-muted-foreground" />
                        )}
                        <span className="sr-only">{showPassword ? "Hide password" : "Show password"}</span>
                      </Button>
                    </div>
                  </FormControl>
                  <PasswordStrength password={password || ""} />
                  <div className="h-[20px]">
                    <FormMessage className="text-xs" />
                  </div>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="confirmPassword"
              render={({ field }) => (
                <FormItem className="form-field h-[90px] mb-2">
                  <FormLabel className="text-foreground">Confirm Password</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                      <Input
                        type={showConfirmPassword ? "text" : "password"}
                        placeholder="••••••••"
                        className="pl-10"
                        autoComplete="new-password"
                        {...field}
                      />
                      <Button
                        type="button"
                        variant="ghost"
                        size="icon"
                        className="absolute right-0 top-0 h-full px-3"
                        onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                      >
                        {showConfirmPassword ? (
                          <EyeOff className="h-4 w-4 text-muted-foreground" />
                        ) : (
                          <Eye className="h-4 w-4 text-muted-foreground" />
                        )}
                        <span className="sr-only">{showConfirmPassword ? "Hide password" : "Show password"}</span>
                      </Button>
                    </div>
                  </FormControl>
                  <div className="h-[20px]">
                    <FormMessage className="text-xs" />
                  </div>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="terms"
              render={({ field }) => (
                <FormItem className="flex items-start space-x-2 space-y-0">
                  <FormControl>
                    <Checkbox checked={field.value} onCheckedChange={field.onChange} />
                  </FormControl>
                  <div className="space-y-1 leading-none">
                    <FormLabel className="text-sm font-normal cursor-pointer text-foreground">
                      I agree to the{" "}
                      <Button variant="link" className="p-0 h-auto text-sm" type="button">
                        Terms of Service
                      </Button>{" "}
                      and{" "}
                      <Button variant="link" className="p-0 h-auto text-sm" type="button">
                        Privacy Policy
                      </Button>
                    </FormLabel>
                    <div className="h-[20px]">
                      <FormMessage className="text-xs" />
                    </div>
                  </div>
                </FormItem>
              )}
            />
            <Button type="submit" className="w-full" disabled={isLoading}>
              {isLoading ? "Creating account..." : "Sign up"}
            </Button>
            <div className="text-center text-sm">
              Already have an account?{" "}
              <Button
                variant="link"
                className="p-0 h-auto"
                type="button"
                onClick={() => {
                  handleOpenChange(false)
                  // Open login modal
                  setTimeout(() => {
                    document.dispatchEvent(new CustomEvent("open-login-modal"))
                  }, 100)
                }}
              >
                Login
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
