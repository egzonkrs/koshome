"use client"

import { useState, useEffect } from "react"
import { useRouter } from "next/navigation"
import { Plus, Building2, TrendingUp, Eye, MessageSquare } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Header } from "@/components/header"
import { ListingsTable } from "@/components/dashboard/listings-table"
import { ListingFormDrawer } from "@/components/dashboard/listing-form-drawer"
import { useUserStore } from "@/store/user-store"
import { useLandlordStore, type LandlordListing } from "@/store/landlord-store"

export default function DashboardPage() {
  const router = useRouter()
  const { user, isAuthenticated } = useUserStore()
  const { listings, getListingsByOwner } = useLandlordStore()

  const [drawerOpen, setDrawerOpen] = useState(false)
  const [editListing, setEditListing] = useState<LandlordListing | null>(null)
  const [mounted, setMounted] = useState(false)

  useEffect(() => {
    setMounted(true)
  }, [])

  // Get user's listings
  const userListings = user ? getListingsByOwner(user.id) : []

  // Calculate stats
  const totalViews = userListings.reduce((sum, l) => sum + l.views, 0)
  const totalInquiries = userListings.reduce((sum, l) => sum + l.inquiries, 0)
  const activeListings = userListings.filter((l) => l.status === "active").length

  const handleEdit = (listing: LandlordListing) => {
    setEditListing(listing)
    setDrawerOpen(true)
  }

  const handleCreateNew = () => {
    setEditListing(null)
    setDrawerOpen(true)
  }

  if (!mounted) {
    return null
  }

  if (!isAuthenticated) {
    return (
      <div className="min-h-screen flex flex-col bg-background">
        <Header />
        <main className="flex-1 container mx-auto px-4 py-16 flex items-center justify-center">
          <Card className="max-w-md w-full">
            <CardHeader className="text-center">
              <CardTitle>Access Denied</CardTitle>
            </CardHeader>
            <CardContent className="text-center space-y-4">
              <p className="text-muted-foreground">Please log in to access your landlord dashboard.</p>
              <Button
                onClick={() => {
                  document.dispatchEvent(new CustomEvent("open-login-modal"))
                }}
              >
                Log In
              </Button>
            </CardContent>
          </Card>
        </main>
      </div>
    )
  }

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1 container mx-auto px-4 py-8">
        {/* Page Header */}
        <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-8">
          <div>
            <h1 className="text-3xl font-bold">Landlord Dashboard</h1>
            <p className="text-muted-foreground">Manage your property listings and track performance</p>
          </div>
          <Button onClick={handleCreateNew} className="gap-2">
            <Plus className="h-4 w-4" />
            Post New Listing
          </Button>
        </div>

        {/* Stats Cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Listings</CardTitle>
              <Building2 className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{userListings.length}</div>
              <p className="text-xs text-muted-foreground">{activeListings} active</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total Views</CardTitle>
              <Eye className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{totalViews.toLocaleString()}</div>
              <p className="text-xs text-muted-foreground">Across all listings</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Inquiries</CardTitle>
              <MessageSquare className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{totalInquiries}</div>
              <p className="text-xs text-muted-foreground">Total messages received</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Conversion Rate</CardTitle>
              <TrendingUp className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                {totalViews > 0 ? ((totalInquiries / totalViews) * 100).toFixed(1) : 0}%
              </div>
              <p className="text-xs text-muted-foreground">Inquiries / Views</p>
            </CardContent>
          </Card>
        </div>

        {/* Listings Table */}
        <div className="space-y-4">
          <h2 className="text-xl font-semibold">Your Listings</h2>
          <ListingsTable listings={userListings} onEdit={handleEdit} />
        </div>
      </main>

      {/* Listing Form Drawer */}
      <ListingFormDrawer open={drawerOpen} onOpenChange={setDrawerOpen} editListing={editListing} />
    </div>
  )
}
