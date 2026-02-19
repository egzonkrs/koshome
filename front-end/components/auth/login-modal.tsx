"use client"

import { useState, useEffect } from "react"
import { useForm } from "react-hook-form"
import type { ZodType } from "zod"
import { Eye, EyeOff, Mail, Lock, AlertCircle } from "lucide-react"
import { loginSchema, type LoginFormValues } from "@/lib/validations/auth"
import { useUserStore } from "@/store/user-store"
import { useToast } from "@/hooks/use-toast"

import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { Checkbox } from "@/components/ui/checkbox"
import { Alert, AlertDescription } from "@/components/ui/alert"

interface LoginModalProps {
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

export function LoginModal({ open, onOpenChange }: LoginModalProps) {
  const [showPassword, setShowPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [serverError, setServerError] = useState<string | null>(null)

  const { login } = useUserStore()
  const { toast } = useToast()

  const form = useForm<LoginFormValues>({
    resolver: safeZodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
      rememberMe: false,
    },
    mode: "onSubmit",
    reValidateMode: "onChange",
  })

  useEffect(() => {
    const handleOpenLoginModal = () => {
      onOpenChange(true)
    }

    document.addEventListener("open-login-modal", handleOpenLoginModal)

    return () => {
      document.removeEventListener("open-login-modal", handleOpenLoginModal)
    }
  }, [onOpenChange])

  async function onSubmit(data: LoginFormValues) {
    setIsLoading(true)
    setServerError(null)

    try {
      const success = await login(data.email, data.password)

      if (success) {
        toast({
          title: "Welcome back!",
          description: "You have successfully logged in.",
        })
        onOpenChange(false)
        form.reset()
      } else {
        setServerError("Invalid email or password. Please try again.")
      }
    } catch (error) {
      console.error("Login failed:", error)
      setServerError("Invalid email or password. Please try again.")
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
      <DialogContent className="sm:max-w-[425px] bg-[#eeefe9] dark:bg-background">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-center">Welcome back</DialogTitle>
          <DialogDescription className="text-center">Enter your credentials to access your account</DialogDescription>
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
                <FormItem className="form-field h-[90px] mb-2">
                  <FormLabel className="text-foreground">Password</FormLabel>
                  <FormControl>
                    <div className="relative">
                      <Lock className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
                      <Input
                        type={showPassword ? "text" : "password"}
                        placeholder="••••••••"
                        className="pl-10"
                        autoComplete="current-password"
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
                  <div className="h-[20px]">
                    <FormMessage className="text-xs" />
                  </div>
                </FormItem>
              )}
            />
            <div className="flex items-center justify-between">
              <FormField
                control={form.control}
                name="rememberMe"
                render={({ field }) => (
                  <FormItem className="flex items-center space-x-2 space-y-0">
                    <FormControl>
                      <Checkbox checked={field.value} onCheckedChange={field.onChange} />
                    </FormControl>
                    <FormLabel className="text-sm font-normal cursor-pointer text-foreground">Remember me</FormLabel>
                  </FormItem>
                )}
              />
              <Button variant="link" className="p-0 h-auto text-sm" type="button">
                Forgot password?
              </Button>
            </div>
            <Button type="submit" className="w-full" disabled={isLoading}>
              {isLoading ? "Logging in..." : "Login"}
            </Button>
            <div className="text-center text-sm">
              Don&apos;t have an account?{" "}
              <Button
                variant="link"
                className="p-0 h-auto"
                type="button"
                onClick={() => {
                  handleOpenChange(false)
                  // Open signup modal
                  setTimeout(() => {
                    document.dispatchEvent(new CustomEvent("open-signup-modal"))
                  }, 100)
                }}
              >
                Sign up
              </Button>
            </div>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  )
}
