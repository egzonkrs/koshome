import type { AuthResponse, CityResponse, CountryResponse, CurrentUser } from "./types"
import { apartments } from "../data"

export const mockApartments = apartments

export const mockCities: CityResponse[] = [
  { id: "city-pr", cityName: "Pristina", cityAlpha3Code: "PRN", countryId: "kos" },
  { id: "city-pe", cityName: "Peja", cityAlpha3Code: "PEJ", countryId: "kos" },
  { id: "city-prz", cityName: "Prizren", cityAlpha3Code: "PRZ", countryId: "kos" },
]

export const mockCountries: CountryResponse[] = [
  { id: "kos", countryName: "Kosovo", alpha3Code: "XKX" },
  { id: "alb", countryName: "Albania", alpha3Code: "ALB" },
]

export const mockUser: CurrentUser = {
  userId: "user-1",
  email: "demo@koshome.com",
  firstName: "Demo",
  lastName: "User",
  roles: ["user"],
}

export const mockAuthResponse: AuthResponse = {
  accessToken: "mock-access-token",
  refreshToken: "mock-refresh-token",
  expiresIn: 3600,
  tokenType: "Bearer",
  userId: mockUser.userId,
  firstName: mockUser.firstName,
  lastName: mockUser.lastName,
  roles: mockUser.roles,
}
