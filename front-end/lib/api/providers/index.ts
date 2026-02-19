/**
 * Data Provider Factory
 *
 * Returns the appropriate provider based on environment configuration.
 */

import { config } from "../../config"
import type { IDataProvider } from "./types"
import { mockApartmentProvider, mockAuthProvider, mockLocationProvider } from "./mock-provider"
import { apiApartmentProvider, apiAuthProvider, apiLocationProvider } from "./api-provider"

const createDataProvider = (): IDataProvider => {
  if (config.features.useMock) {
    console.log("[Data] Using mock data provider")
    return {
      apartments: mockApartmentProvider,
      auth: mockAuthProvider,
      locations: mockLocationProvider,
    }
  }

  console.log("[Data] Using real API provider")
  return {
    apartments: apiApartmentProvider,
    auth: apiAuthProvider,
    locations: apiLocationProvider,
  }
}

export const dataProvider = createDataProvider()

export const apartmentProvider = dataProvider.apartments
export const authProvider = dataProvider.auth
export const locationProvider = dataProvider.locations

export * from "./types"
