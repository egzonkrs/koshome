import { create } from "zustand"
import { persist } from "zustand/middleware"
import type { Apartment } from "@/lib/types"

export type ListingStatus = "active" | "pending" | "inactive" | "sold" | "rented"

export interface LandlordListing extends Apartment {
  status: ListingStatus
  views: number
  inquiries: number
  ownerId: string
}

interface LandlordState {
  listings: LandlordListing[]

  // Actions
  addListing: (listing: Omit<LandlordListing, "id" | "createdAt" | "updatedAt" | "views" | "inquiries">) => void
  updateListing: (id: string, updates: Partial<LandlordListing>) => void
  deleteListing: (id: string) => void
  getListingsByOwner: (ownerId: string) => LandlordListing[]
}

// Mock initial listings for the demo
const initialListings: LandlordListing[] = [
  {
    id: "landlord-apt-001",
    title: "Sunny Downtown Apartment",
    description: "Beautiful sunny apartment in the heart of downtown. Perfect for professionals.",
    price: 450,
    pricePerSqm: 9,
    area: 50,
    bedrooms: 1,
    bathrooms: 1,
    floor: 3,
    location: {
      address: "45 Main Street",
      city: "Pristina",
      district: "City Center",
      country: "Kosovo",
      coordinates: [42.6629, 21.1655],
    },
    features: ["Furnished", "Air Conditioning", "Internet", "Balcony"],
    images: [
      "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: false,
    forRent: true,
    createdAt: "2024-01-15T10:30:00Z",
    updatedAt: "2024-01-15T10:30:00Z",
    status: "active",
    views: 234,
    inquiries: 12,
    ownerId: "user-1",
  },
  {
    id: "landlord-apt-002",
    title: "Spacious Family Home",
    description: "Large family apartment with garden access. Great for families with children.",
    price: 125000,
    pricePerSqm: 1250,
    area: 100,
    bedrooms: 3,
    bathrooms: 2,
    floor: 0,
    location: {
      address: "78 Garden Lane",
      city: "Pristina",
      district: "Arbëri",
      country: "Kosovo",
      coordinates: [42.6729, 21.1755],
    },
    features: ["Garden", "Parking", "Storage", "Pet Friendly"],
    images: [
      "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2024-02-10T14:20:00Z",
    updatedAt: "2024-02-10T14:20:00Z",
    status: "active",
    views: 156,
    inquiries: 8,
    ownerId: "user-1",
  },
  {
    id: "landlord-apt-003",
    title: "Modern Studio Near University",
    description: "Compact modern studio, ideal for students or young professionals.",
    price: 280,
    pricePerSqm: 7,
    area: 40,
    bedrooms: 0,
    bathrooms: 1,
    floor: 2,
    location: {
      address: "12 University Road",
      city: "Pristina",
      district: "Dardania",
      country: "Kosovo",
      coordinates: [42.6529, 21.1555],
    },
    features: ["Furnished", "Internet", "Laundry"],
    images: [
      "https://images.unsplash.com/photo-1554995207-c18c203602cb?ixlib=rb-4.0.3&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: false,
    forRent: true,
    createdAt: "2024-03-05T09:00:00Z",
    updatedAt: "2024-03-05T09:00:00Z",
    status: "pending",
    views: 89,
    inquiries: 5,
    ownerId: "user-1",
  },
]

export const useLandlordStore = create<LandlordState>()(
  persist(
    (set, get) => ({
      listings: initialListings,

      addListing: (listing) => {
        const newListing: LandlordListing = {
          ...listing,
          id: `landlord-apt-${Date.now()}`,
          createdAt: new Date().toISOString(),
          updatedAt: new Date().toISOString(),
          views: 0,
          inquiries: 0,
        }
        set((state) => ({
          listings: [newListing, ...state.listings],
        }))
      },

      updateListing: (id, updates) => {
        set((state) => ({
          listings: state.listings.map((listing) =>
            listing.id === id ? { ...listing, ...updates, updatedAt: new Date().toISOString() } : listing,
          ),
        }))
      },

      deleteListing: (id) => {
        set((state) => ({
          listings: state.listings.filter((listing) => listing.id !== id),
        }))
      },

      getListingsByOwner: (ownerId) => {
        return get().listings.filter((listing) => listing.ownerId === ownerId)
      },
    }),
    {
      name: "koshome-landlord",
    },
  ),
)
