"use client"

import { useState, useEffect } from "react"
import Link from "next/link"
import { usePathname } from "next/navigation"
import { useTheme } from "next-themes"
import { Home, Menu, X, Moon, Sun, Coins, LayoutDashboard, LogOut } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"
import { LoginModal } from "@/components/auth/login-modal"
import { SignupModal } from "@/components/auth/signup-modal"
import { useUserStore } from "@/store/user-store"
import { cn } from "@/lib/utils"

export function Header() {
  const { resolvedTheme, setTheme } = useTheme()
  const [mounted, setMounted] = useState(false)
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  const [loginModalOpen, setLoginModalOpen] = useState(false)
  const [signupModalOpen, setSignupModalOpen] = useState(false)
  const pathname = usePathname()

  const { user, isAuthenticated, logout } = useUserStore()

  // Fix hydration issues with theme
  useEffect(() => {
    setMounted(true)
  }, [])

  useEffect(() => {
    const handleOpenLoginModal = () => {
      setLoginModalOpen(true)
    }

    document.addEventListener("open-login-modal", handleOpenLoginModal)

    return () => {
      document.removeEventListener("open-login-modal", handleOpenLoginModal)
    }
  }, [])

  const toggleTheme = () => {
    setTheme(resolvedTheme === "dark" ? "light" : "dark")
  }

  const isActive = (path: string) => {
    return pathname === path
  }

  const handleLogout = () => {
    logout()
    setMobileMenuOpen(false)
  }

  return (
    <header className="bg-background/80 backdrop-blur-md border-b sticky top-0 z-40">
      <div className="w-full px-4 flex items-center justify-between h-16">
        {/* Logo */}
        <Link href="/" className="flex items-center space-x-2">
          <div className="bg-primary p-1.5 rounded-md">
            <Home className="h-5 w-5 text-primary-foreground" />
          </div>
          <span className="font-bold text-xl hidden sm:inline-block">KosHome</span>
        </Link>

        {/* Desktop Navigation */}
        <nav className="hidden md:flex items-center space-x-6">
          <Link
            href="/"
            className={cn(
              "text-sm font-medium transition-colors",
              isActive("/") ? "text-primary" : "hover:text-primary",
            )}
          >
            Home
          </Link>
          <Link
            href="/apartments"
            className={cn(
              "text-sm font-medium transition-colors",
              isActive("/apartments") ? "text-primary" : "hover:text-primary",
            )}
          >
            Apartments
          </Link>
          <Link
            href="/about"
            className={cn(
              "text-sm font-medium transition-colors",
              isActive("/about") ? "text-primary" : "hover:text-primary",
            )}
          >
            About
          </Link>
          <Link
            href="/contact"
            className={cn(
              "text-sm font-medium transition-colors",
              isActive("/contact") ? "text-primary" : "hover:text-primary",
            )}
          >
            Contact
          </Link>
          <Link
            href="/pricing"
            className={cn(
              "text-sm font-medium transition-colors",
              isActive("/pricing") ? "text-primary" : "hover:text-primary",
            )}
          >
            Pricing
          </Link>
        </nav>

        {/* Auth Buttons / User Menu */}
        <div className="hidden md:flex items-center space-x-4">
          {isAuthenticated && user ? (
            <>
              {/* Credits Badge */}
              <Link href="/pricing">
                <Badge variant="secondary" className="gap-1 cursor-pointer hover:bg-secondary/80">
                  <Coins className="h-3 w-3" />
                  Credits: {user.plan === "premium" ? "∞" : user.credits}
                </Badge>
              </Link>

              {/* User Dropdown */}
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button variant="ghost" className="gap-2 pl-2 pr-3">
                    <Avatar className="h-7 w-7">
                      <AvatarFallback className="bg-primary text-primary-foreground text-xs">
                        {user.name.charAt(0).toUpperCase()}
                      </AvatarFallback>
                    </Avatar>
                    <span className="hidden lg:inline-block">{user.name}</span>
                  </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-48">
                  <DropdownMenuItem asChild>
                    <Link href="/dashboard" className="cursor-pointer">
                      <LayoutDashboard className="h-4 w-4 mr-2" />
                      Dashboard
                    </Link>
                  </DropdownMenuItem>
                  <DropdownMenuItem asChild>
                    <Link href="/pricing" className="cursor-pointer">
                      <Coins className="h-4 w-4 mr-2" />
                      Upgrade Plan
                    </Link>
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem onClick={handleLogout} className="text-destructive cursor-pointer">
                    <LogOut className="h-4 w-4 mr-2" />
                    Log Out
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </>
          ) : (
            <>
              <Button variant="outline" onClick={() => setLoginModalOpen(true)}>
                Login
              </Button>
              <Button onClick={() => setSignupModalOpen(true)}>Sign up</Button>
            </>
          )}

          {mounted && (
            <Button variant="ghost" size="icon" onClick={toggleTheme} className="ml-2">
              {resolvedTheme === "dark" ? <Sun className="h-5 w-5" /> : <Moon className="h-5 w-5" />}
              <span className="sr-only">Toggle theme</span>
            </Button>
          )}
        </div>

        {/* Mobile Menu Button */}
        <div className="flex md:hidden items-center space-x-2">
          {isAuthenticated && user && (
            <Badge variant="secondary" className="gap-1">
              <Coins className="h-3 w-3" />
              {user.plan === "premium" ? "∞" : user.credits}
            </Badge>
          )}
          {mounted && (
            <Button variant="ghost" size="icon" onClick={toggleTheme}>
              {resolvedTheme === "dark" ? <Sun className="h-5 w-5" /> : <Moon className="h-5 w-5" />}
              <span className="sr-only">Toggle theme</span>
            </Button>
          )}
          <Button variant="ghost" size="icon" onClick={() => setMobileMenuOpen(!mobileMenuOpen)}>
            {mobileMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
            <span className="sr-only">Toggle menu</span>
          </Button>
        </div>
      </div>

      {/* Mobile Menu */}
      {mobileMenuOpen && (
        <div className="md:hidden bg-background/95 backdrop-blur-md border-b absolute w-full z-50">
          <div className="container mx-auto px-4 py-4 space-y-4">
            <nav className="flex flex-col space-y-4">
              <Link
                href="/"
                className={cn("text-sm font-medium", isActive("/") ? "text-primary" : "hover:text-primary")}
                onClick={() => setMobileMenuOpen(false)}
              >
                Home
              </Link>
              <Link
                href="/apartments"
                className={cn("text-sm font-medium", isActive("/apartments") ? "text-primary" : "hover:text-primary")}
                onClick={() => setMobileMenuOpen(false)}
              >
                Apartments
              </Link>
              <Link
                href="/about"
                className={cn("text-sm font-medium", isActive("/about") ? "text-primary" : "hover:text-primary")}
                onClick={() => setMobileMenuOpen(false)}
              >
                About
              </Link>
              <Link
                href="/contact"
                className={cn("text-sm font-medium", isActive("/contact") ? "text-primary" : "hover:text-primary")}
                onClick={() => setMobileMenuOpen(false)}
              >
                Contact
              </Link>
              <Link
                href="/pricing"
                className={cn("text-sm font-medium", isActive("/pricing") ? "text-primary" : "hover:text-primary")}
                onClick={() => setMobileMenuOpen(false)}
              >
                Pricing
              </Link>
              {isAuthenticated && (
                <Link
                  href="/dashboard"
                  className={cn("text-sm font-medium", isActive("/dashboard") ? "text-primary" : "hover:text-primary")}
                  onClick={() => setMobileMenuOpen(false)}
                >
                  Dashboard
                </Link>
              )}
            </nav>
            <div className="flex space-x-4 pt-2 border-t">
              {isAuthenticated && user ? (
                <Button variant="outline" className="flex-1 bg-transparent" onClick={handleLogout}>
                  <LogOut className="h-4 w-4 mr-2" />
                  Log Out
                </Button>
              ) : (
                <>
                  <Button
                    variant="outline"
                    className="flex-1 bg-transparent"
                    onClick={() => {
                      setLoginModalOpen(true)
                      setMobileMenuOpen(false)
                    }}
                  >
                    Login
                  </Button>
                  <Button
                    className="flex-1"
                    onClick={() => {
                      setSignupModalOpen(true)
                      setMobileMenuOpen(false)
                    }}
                  >
                    Sign up
                  </Button>
                </>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Auth Modals */}
      <LoginModal open={loginModalOpen} onOpenChange={setLoginModalOpen} />
      <SignupModal open={signupModalOpen} onOpenChange={setSignupModalOpen} />
    </header>
  )
}
