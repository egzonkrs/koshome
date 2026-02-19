/**
 * Data Provider Interface
 *
 * Defines the contract that both Mock and Real providers must implement.
 */

import type {
  ApartmentResponse,
  ApartmentFilterParams,
  CreateApartmentRequest,
  PaginatedResponse,
  ApiResponse,
  AuthResponse,
  LoginRequest,
  RegisterRequest,
  CurrentUser,
  CityResponse,
  CountryResponse,
} from "../types"

export interface IApartmentProvider {
  getApartments(params?: ApartmentFilterParams): Promise<ApiResponse<PaginatedResponse<ApartmentResponse>>>
  getApartmentById(id: string): Promise<ApartmentResponse | null>
  createApartment(data: CreateApartmentRequest): Promise<string>
}

export interface IAuthProvider {
  login(data: LoginRequest): Promise<AuthResponse>
  register(data: RegisterRequest): Promise<AuthResponse>
  logout(): Promise<void>
  getCurrentUser(): Promise<CurrentUser>
}

export interface ILocationProvider {
  getCities(): Promise<CityResponse[]>
  getCountries(): Promise<CountryResponse[]>
}

// Combined provider for convenience
export interface IDataProvider {
  apartments: IApartmentProvider
  auth: IAuthProvider
  locations: ILocationProvider
}
