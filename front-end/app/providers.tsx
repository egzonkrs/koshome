"use client"

import type React from "react"
import { QueryClientProvider } from "@tanstack/react-query"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { queryClient } from "@/lib/query"
import { config } from "@/lib/config"

export function AppProviders({ children }: { children: React.ReactNode }) {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {config.features.enableDevtools ? <ReactQueryDevtools initialIsOpen={false} /> : null}
    </QueryClientProvider>
  )
}
