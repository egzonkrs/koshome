/**
 * Mock Data Provider
 *
 * Provides mock data for development and testing.
 */

import { config } from "../../config"
import type { IApartmentProvider, IAuthProvider, ILocationProvider } from "./types"
import { mockApartments, mockCities, mockCountries, mockUser, mockAuthResponse } from "../mock-data"
import type {
  ApartmentFilterParams,
  ApartmentResponse,
  PaginatedResponse,
  ApiResponse,
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  CurrentUser,
  CityResponse,
  CountryResponse,
  CreateApartmentRequest,
  ApartmentLocation,
} from "../types"

const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms))

const defaultLocation: ApartmentLocation = {
  address: "",
  city: "Unknown",
  district: "Unknown",
  country: "Kosovo",
  coordinates: [42.6629, 21.1655],
}

const getLocation = (apartment: ApartmentResponse): ApartmentLocation => {
  if (apartment.location) {
    return apartment.location
  }

  return {
    ...defaultLocation,
    address: apartment.address || defaultLocation.address,
    coordinates: [apartment.latitude ?? defaultLocation.coordinates[0], apartment.longitude ?? defaultLocation.coordinates[1]],
  }
}

const getArea = (apartment: ApartmentResponse): number => {
  return apartment.area ?? apartment.squareMeters ?? 0
}

const isPointInPolygon = (point: [number, number], polygon: [number, number][]): boolean => {
  const [x, y] = point
  let inside = false

  for (let i = 0, j = polygon.length - 1; i < polygon.length; j = i++) {
    const [xi, yi] = polygon[i]
    const [xj, yj] = polygon[j]

    if (yi > y !== yj > y && x < ((xj - xi) * (y - yi)) / (yj - yi) + xi) {
      inside = !inside
    }
  }

  return inside
}

export const mockApartmentProvider: IApartmentProvider = {
  async getApartments(params?: ApartmentFilterParams): Promise<ApiResponse<PaginatedResponse<ApartmentResponse>>> {
    await delay(300)

    let filtered = [...mockApartments]

    const minPrice = params?.minPrice ?? params?.priceMin
    const maxPrice = params?.maxPrice ?? params?.priceMax
    const minArea = params?.minArea ?? params?.areaMin
    const maxArea = params?.maxArea ?? params?.areaMax

    if (minPrice !== undefined) {
      filtered = filtered.filter((apt) => apt.price >= minPrice)
    }
    if (maxPrice !== undefined) {
      filtered = filtered.filter((apt) => apt.price <= maxPrice)
    }
    if (params?.bedrooms !== undefined) {
      filtered = filtered.filter((apt) => apt.bedrooms >= params.bedrooms!)
    }
    if (params?.bathrooms !== undefined) {
      filtered = filtered.filter((apt) => apt.bathrooms >= params.bathrooms!)
    }
    if (minArea !== undefined) {
      filtered = filtered.filter((apt) => getArea(apt) >= minArea)
    }
    if (maxArea !== undefined) {
      filtered = filtered.filter((apt) => getArea(apt) <= maxArea)
    }
    if (params?.forSale !== undefined && params.forSale) {
      filtered = filtered.filter((apt) => {
        const listingType = apt.listingType?.toString().toLowerCase()
        return apt.forSale ?? listingType === "sale"
      })
    }
    if (params?.forRent !== undefined && params.forRent) {
      filtered = filtered.filter((apt) => {
        const listingType = apt.listingType?.toString().toLowerCase()
        return apt.forRent ?? listingType === "rent"
      })
    }
    if (params?.district) {
      const district = params.district.toLowerCase()
      filtered = filtered.filter((apt) => getLocation(apt).district.toLowerCase() === district)
    }
    if (params?.polygonBounds && params.polygonBounds.length >= 3) {
      filtered = filtered.filter((apt) => {
        const location = getLocation(apt)
        return isPointInPolygon(location.coordinates, params.polygonBounds!)
      })
    }
    if (params?.search) {
      const search = params.search.toLowerCase()
      filtered = filtered.filter((apt) => {
        const location = getLocation(apt)
        const features = apt.features ?? []
        return (
          (apt.title ?? "").toLowerCase().includes(search) ||
          (apt.description ?? "").toLowerCase().includes(search) ||
          location.address.toLowerCase().includes(search) ||
          location.city.toLowerCase().includes(search) ||
          location.district.toLowerCase().includes(search) ||
          features.some((feature) => feature.toLowerCase().includes(search))
        )
      })
    }

    const pageNumber = params?.pageNumber || 1
    const pageSize = params?.pageSize || 10
    const startIndex = (pageNumber - 1) * pageSize
    const paginatedItems = filtered.slice(startIndex, startIndex + pageSize)
    const totalPages = Math.ceil(filtered.length / pageSize)

    return {
      data: {
        items: paginatedItems,
        pagination: {
          pageNumber,
          pageSize,
          totalCount: filtered.length,
          totalPages,
          hasNextPage: pageNumber < totalPages,
          hasPreviousPage: pageNumber > 1,
          currentPageSize: paginatedItems.length,
          isFirstPage: pageNumber === 1,
          isLastPage: pageNumber >= totalPages,
        },
      },
      isFailed: false,
      isSuccess: true,
      reasons: null,
      errors: null,
    }
  },

  async getApartmentById(id: string): Promise<ApartmentResponse | null> {
    await delay(200)
    const apartment = mockApartments.find((apt) => apt.id === id)
    return apartment ?? null
  },

  async createApartment(data: CreateApartmentRequest): Promise<string> {
    await delay(500)
    const newId = `apt-${Date.now()}`
    console.log("Mock: Created apartment", newId, data)
    return newId
  },
}

export const mockAuthProvider: IAuthProvider = {
  async login(data: LoginRequest): Promise<AuthResponse> {
    await delay(500)

    if (typeof window !== "undefined") {
      localStorage.setItem(config.auth.tokenKey, mockAuthResponse.accessToken || "")
      localStorage.setItem(
        config.auth.userKey,
        JSON.stringify({
          ...mockUser,
          email: data.email,
        }),
      )
    }

    return mockAuthResponse
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    await delay(700)

    const userId = `user-${Date.now()}`
    const response: AuthResponse = {
      ...mockAuthResponse,
      userId,
      firstName: data.firstName,
      lastName: data.lastName,
    }

    if (typeof window !== "undefined") {
      localStorage.setItem(config.auth.tokenKey, mockAuthResponse.accessToken || "")
      localStorage.setItem(
        config.auth.userKey,
        JSON.stringify({
          userId,
          email: data.email,
          firstName: data.firstName,
          lastName: data.lastName,
          roles: ["user"],
        }),
      )
    }

    return response
  },

  async logout(): Promise<void> {
    await delay(200)
    if (typeof window !== "undefined") {
      localStorage.removeItem(config.auth.tokenKey)
      localStorage.removeItem(config.auth.userKey)
    }
  },

  async getCurrentUser(): Promise<CurrentUser> {
    await delay(200)
    if (typeof window !== "undefined") {
      const stored = localStorage.getItem(config.auth.userKey)
      if (stored) {
        return JSON.parse(stored)
      }
    }
    return mockUser
  },
}

export const mockLocationProvider: ILocationProvider = {
  async getCities(): Promise<CityResponse[]> {
    await delay(200)
    return mockCities
  },

  async getCountries(): Promise<CountryResponse[]> {
    await delay(200)
    return mockCountries
  },
}
