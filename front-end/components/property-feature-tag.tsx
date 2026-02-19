import type React from "react"
import { cn } from "@/lib/utils"

interface PropertyFeatureTagProps {
  children: React.ReactNode
  className?: string
}

export function PropertyFeatureTag({ children, className }: PropertyFeatureTagProps) {
  return (
    <div className={cn("bg-muted/50 text-sm px-3 py-1.5 rounded-full border border-border/50", className)}>
      {children}
    </div>
  )
}
