/**
 * Apartment Types
 *
 * These types are the SINGLE SOURCE OF TRUTH for apartment data.
 * They match the backend DTOs with optional fields for flexibility.
 */

export interface ApartmentResponse {
  id: string
  title?: string | null
  description?: string | null
  price: number
  bedrooms: number
  bathrooms: number
  createdAt: string
  updatedAt: string

  // These may be missing from list endpoints but present in detail
  pricePerSqm?: number
  squareMeters?: number
  area?: number // Alias for squareMeters (UI compatibility)
  floor?: number

  // Address can come as string or object
  address?: string
  cityId?: string
  latitude?: number
  longitude?: number

  // Listing type
  listingType?: "sale" | "rent" | "Sale" | "Rent" | string | null
  forSale?: boolean
  forRent?: boolean

  // Optional nested location (for UI compatibility)
  location?: ApartmentLocation

  // Arrays that may not be in list responses
  features?: string[]
  images?: string[]
}

export interface ApartmentLocation {
  address: string
  city: string
  district: string
  country: string
  coordinates: [number, number] // [lat, lng]
}

// Filter parameters for API calls
export interface ApartmentFilterParams {
  pageNumber?: number
  pageSize?: number
  cityId?: string
  minPrice?: number
  maxPrice?: number
  minArea?: number
  maxArea?: number
  priceMin?: number
  priceMax?: number
  areaMin?: number
  areaMax?: number
  bedrooms?: number
  bathrooms?: number
  forSale?: boolean
  forRent?: boolean
  search?: string

  // UI-only filters
  district?: string
  polygonBounds?: [number, number][]
}

// Create request (what we send to backend)
export interface CreateApartmentRequest {
  title: string
  description: string
  price: number
  listingType: string
  propertyType: string
  address: string
  cityId: string
  bedrooms: number
  bathrooms: number
  squareMeters: number
  latitude: number
  longitude: number
  images?: File[]
}
