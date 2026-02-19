"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { Check, Sparkles, Building2, Zap } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Header } from "@/components/header"
import { useUserStore, type UserPlan } from "@/store/user-store"
import { useToast } from "@/hooks/use-toast"
import { cn } from "@/lib/utils"

interface PlanFeature {
  text: string
  included: boolean
}

interface Plan {
  id: UserPlan
  name: string
  price: string
  period: string
  description: string
  features: PlanFeature[]
  popular?: boolean
  icon: React.ReactNode
}

const plans: Plan[] = [
  {
    id: "free",
    name: "Free",
    price: "€0",
    period: "forever",
    description: "Get started with basic listing features",
    icon: <Building2 className="h-5 w-5" />,
    features: [
      { text: "1 Property Listing", included: true },
      { text: "Basic Property Details", included: true },
      { text: "Email Notifications", included: true },
      { text: "AI Description Generator", included: false },
      { text: "Market Value Analysis", included: false },
      { text: "Priority Support", included: false },
    ],
  },
  {
    id: "standard",
    name: "Standard",
    price: "€9",
    period: "/month",
    description: "Perfect for individual landlords",
    icon: <Sparkles className="h-5 w-5" />,
    popular: true,
    features: [
      { text: "5 Property Listings", included: true },
      { text: "All Property Details", included: true },
      { text: "Email + SMS Notifications", included: true },
      { text: "10 AI Credits/month", included: true },
      { text: "Market Value Analysis", included: true },
      { text: "Email Support", included: true },
    ],
  },
  {
    id: "premium",
    name: "Premium",
    price: "€29",
    period: "/month",
    description: "For professional property managers",
    icon: <Zap className="h-5 w-5" />,
    features: [
      { text: "Unlimited Listings", included: true },
      { text: "All Property Details", included: true },
      { text: "All Notification Channels", included: true },
      { text: "Unlimited AI Credits", included: true },
      { text: "Advanced Analytics", included: true },
      { text: "Priority 24/7 Support", included: true },
    ],
  },
]

export default function PricingPage() {
  const router = useRouter()
  const { toast } = useToast()
  const { user, isAuthenticated, upgradePlan } = useUserStore()
  const [loading, setLoading] = useState<UserPlan | null>(null)

  const handleSelectPlan = async (planId: UserPlan) => {
    if (!isAuthenticated) {
      document.dispatchEvent(new CustomEvent("open-login-modal"))
      return
    }

    if (user?.plan === planId) {
      toast({
        title: "Already on this plan",
        description: `You're already subscribed to the ${planId} plan.`,
      })
      return
    }

    setLoading(planId)

    // Simulate API call
    await new Promise((resolve) => setTimeout(resolve, 1500))

    upgradePlan(planId)
    setLoading(null)

    toast({
      title: "Plan Updated!",
      description: `You've successfully ${planId === "free" ? "downgraded to" : "upgraded to"} the ${planId.charAt(0).toUpperCase() + planId.slice(1)} plan.`,
    })

    router.push("/dashboard")
  }

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1 container mx-auto px-4 py-16">
        {/* Page Header */}
        <div className="text-center max-w-2xl mx-auto mb-12">
          <Badge variant="secondary" className="mb-4">
            Pricing Plans
          </Badge>
          <h1 className="text-4xl font-bold mb-4">Choose the Right Plan for You</h1>
          <p className="text-lg text-muted-foreground">
            Whether you're just starting out or managing multiple properties, we have a plan that fits your needs.
          </p>
        </div>

        {/* Pricing Cards */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 max-w-5xl mx-auto">
          {plans.map((plan) => (
            <Card
              key={plan.id}
              className={cn("relative flex flex-col", plan.popular && "border-primary shadow-lg scale-105")}
            >
              {plan.popular && <Badge className="absolute -top-3 left-1/2 -translate-x-1/2">Most Popular</Badge>}

              <CardHeader>
                <div className="flex items-center gap-2 mb-2">
                  <div
                    className={cn("p-2 rounded-lg", plan.popular ? "bg-primary text-primary-foreground" : "bg-muted")}
                  >
                    {plan.icon}
                  </div>
                  <CardTitle>{plan.name}</CardTitle>
                </div>
                <div className="flex items-baseline gap-1">
                  <span className="text-4xl font-bold">{plan.price}</span>
                  <span className="text-muted-foreground">{plan.period}</span>
                </div>
                <CardDescription>{plan.description}</CardDescription>
              </CardHeader>

              <CardContent className="flex-1">
                <ul className="space-y-3">
                  {plan.features.map((feature, index) => (
                    <li key={index} className="flex items-center gap-2">
                      <div
                        className={cn(
                          "h-5 w-5 rounded-full flex items-center justify-center",
                          feature.included ? "bg-primary/10 text-primary" : "bg-muted text-muted-foreground",
                        )}
                      >
                        <Check className="h-3 w-3" />
                      </div>
                      <span className={cn("text-sm", !feature.included && "text-muted-foreground line-through")}>
                        {feature.text}
                      </span>
                    </li>
                  ))}
                </ul>
              </CardContent>

              <CardFooter>
                <Button
                  className="w-full"
                  variant={plan.popular ? "default" : "outline"}
                  disabled={loading !== null || user?.plan === plan.id}
                  onClick={() => handleSelectPlan(plan.id)}
                >
                  {loading === plan.id
                    ? "Processing..."
                    : user?.plan === plan.id
                      ? "Current Plan"
                      : plan.id === "free"
                        ? "Get Started"
                        : "Upgrade Now"}
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>

        {/* FAQ Section */}
        <div className="mt-16 max-w-2xl mx-auto text-center">
          <h2 className="text-2xl font-bold mb-4">Frequently Asked Questions</h2>
          <div className="space-y-4 text-left">
            <div>
              <h3 className="font-semibold mb-1">What are AI Credits?</h3>
              <p className="text-sm text-muted-foreground">
                AI Credits are used for generating listing descriptions and checking market values. Each action consumes
                1 credit.
              </p>
            </div>
            <div>
              <h3 className="font-semibold mb-1">Can I change plans anytime?</h3>
              <p className="text-sm text-muted-foreground">
                Yes! You can upgrade or downgrade your plan at any time. Changes take effect immediately.
              </p>
            </div>
            <div>
              <h3 className="font-semibold mb-1">Is there a contract?</h3>
              <p className="text-sm text-muted-foreground">
                No long-term contracts. All plans are billed monthly and you can cancel anytime.
              </p>
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
