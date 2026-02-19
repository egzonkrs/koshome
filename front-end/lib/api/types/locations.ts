/**
 * Location Types
 */

export interface CityResponse {
  id: string
  cityName?: string | null
  cityAlpha3Code?: string | null
  countryId?: string
}

export interface CountryResponse {
  id: string
  countryName?: string | null
  alpha3Code?: string | null
}
