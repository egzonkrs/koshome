import type React from "react"
import type { Metadata } from "next"
import { Poppins } from "next/font/google"
import "./globals.css"
import { ThemeProvider } from "@/components/theme-provider"
import { AppProviders } from "./providers"

const poppins = Poppins({
  subsets: ["latin"],
  weight: ["300", "400", "500", "600", "700"],
  variable: "--font-poppins",
})

export const metadata: Metadata = {
  title: "KosHome - Find Your Perfect Home",
  description: "Browse apartments and find your perfect home with our interactive map",
    generator: 'v0.app'
}

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode
}>) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className={`${poppins.variable} font-sans`}>
        <AppProviders>
          <ThemeProvider attribute="class" defaultTheme="light" enableSystem>
            {children}
          </ThemeProvider>
        </AppProviders>
      </body>
    </html>
  )
}
