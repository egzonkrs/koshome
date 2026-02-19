/**
 * Real API Provider
 *
 * Connects to the actual backend API.
 */

import type { IApartmentProvider, IAuthProvider, ILocationProvider } from "./types"
import { apiClient } from "../client"
import { config } from "../../config"
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
} from "../types"

const toApiParams = (params?: ApartmentFilterParams) => {
  if (!params) return undefined

  const {
    pageNumber,
    pageSize,
    cityId,
    minPrice,
    maxPrice,
    minArea,
    maxArea,
    priceMin,
    priceMax,
    areaMin,
    areaMax,
    bedrooms,
    bathrooms,
    forSale,
    forRent,
    search,
  } = params

  const resolvedMinPrice = minPrice ?? priceMin
  const resolvedMaxPrice = maxPrice ?? priceMax
  const resolvedMinArea = minArea ?? areaMin
  const resolvedMaxArea = maxArea ?? areaMax

  const apiParams: Record<string, unknown> = {
    pageNumber,
    pageSize,
    cityId,
    minPrice: resolvedMinPrice,
    maxPrice: resolvedMaxPrice,
    minArea: resolvedMinArea,
    maxArea: resolvedMaxArea,
    bedrooms,
    bathrooms,
    forSale,
    forRent,
    search,
  }

  Object.keys(apiParams).forEach((key) => {
    if (apiParams[key] === undefined) {
      delete apiParams[key]
    }
  })

  return apiParams
}

export const apiApartmentProvider: IApartmentProvider = {
  async getApartments(params?: ApartmentFilterParams): Promise<ApiResponse<PaginatedResponse<ApartmentResponse>>> {
    const response = await apiClient.get<ApiResponse<PaginatedResponse<ApartmentResponse>>>("/apartments", {
      params: toApiParams(params),
    })
    return response.data
  },

  async getApartmentById(id: string): Promise<ApartmentResponse | null> {
    const pageSize = 50
    let pageNumber = 1
    let hasNext = true

    while (hasNext && pageNumber <= 10) {
      const response = await apiClient.get<ApiResponse<PaginatedResponse<ApartmentResponse>>>("/apartments", {
        params: toApiParams({ pageNumber, pageSize }),
      })

      const items = response.data.data?.items ?? []
      const found = items.find((item) => item.id === id)
      if (found) {
        return found
      }

      const pagination = response.data.data?.pagination
      hasNext = pagination ? pagination.hasNextPage : false
      pageNumber += 1
    }

    return null
  },

  async createApartment(data: CreateApartmentRequest): Promise<string> {
    const formData = new FormData()
    formData.append("Title", data.title)
    formData.append("Description", data.description)
    formData.append("Price", String(data.price))
    formData.append("ListingType", data.listingType)
    formData.append("PropertyType", data.propertyType)
    formData.append("Address", data.address)
    formData.append("CityId", data.cityId)
    formData.append("Bedrooms", String(data.bedrooms))
    formData.append("Bathrooms", String(data.bathrooms))
    formData.append("SquareMeters", String(data.squareMeters))
    formData.append("Latitude", String(data.latitude))
    formData.append("Longitude", String(data.longitude))
    if (data.images && data.images.length > 0) {
      data.images.forEach((file) => formData.append("Images", file))
    }

    const response = await apiClient.post<string>("/apartments", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
    return response.data
  },
}

export const apiAuthProvider: IAuthProvider = {
  async login(data: LoginRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse | ApiResponse<AuthResponse>>("/auth/login", data)
    const payload = "isSuccess" in response.data ? response.data.data : response.data

    if (payload?.accessToken && typeof window !== "undefined") {
      localStorage.setItem(config.auth.tokenKey, payload.accessToken)
      if (payload.refreshToken) {
        localStorage.setItem(config.auth.refreshTokenKey, payload.refreshToken)
      }
      if (payload.userId) {
        localStorage.setItem(
          config.auth.userKey,
          JSON.stringify({
            userId: payload.userId,
            email: data.email,
            firstName: payload.firstName || "",
            lastName: payload.lastName || "",
            roles: payload.roles || [],
          }),
        )
      }
    }

    return payload ?? {}
  },

  async register(data: RegisterRequest): Promise<AuthResponse> {
    const response = await apiClient.post<AuthResponse | ApiResponse<AuthResponse>>("/auth/signup", data)
    const payload = "isSuccess" in response.data ? response.data.data : response.data
    return payload ?? {}
  },

  async logout(): Promise<void> {
    if (typeof window !== "undefined") {
      localStorage.removeItem(config.auth.tokenKey)
      localStorage.removeItem(config.auth.refreshTokenKey)
      localStorage.removeItem(config.auth.userKey)
    }
  },

  async getCurrentUser(): Promise<CurrentUser> {
    if (typeof window !== "undefined") {
      const stored = localStorage.getItem(config.auth.userKey)
      if (stored) {
        return JSON.parse(stored)
      }
    }
    return {
      userId: "",
      email: "",
      firstName: "",
      lastName: "",
      roles: [],
    }
  },
}

export const apiLocationProvider: ILocationProvider = {
  async getCities(): Promise<CityResponse[]> {
    const response = await apiClient.get<ApiResponse<PaginatedResponse<CityResponse>>>("/cities")
    return response.data.data?.items ?? []
  },

  async getCountries(): Promise<CountryResponse[]> {
    const response = await apiClient.get<CountryResponse[]>("/countries")
    return response.data ?? []
  },
}
