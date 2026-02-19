import { create } from "zustand"
import type { Apartment, ApartmentFilters, ApartmentLocation } from "@/lib/types"
import { config } from "@/lib/config"
import { mockApartments } from "@/lib/api/mock-data"

interface ApartmentState {
  apartments: Apartment[]
  filteredApartments: Apartment[]
  selectedApartment: Apartment | null
  filters: ApartmentFilters
  isDarkMode: boolean
  sortBy: "price-asc" | "price-desc" | "area-asc" | "area-desc" | "newest" | "oldest"

  // Actions
  setApartments: (apartments: Apartment[]) => void
  setSelectedApartment: (apartment: Apartment | null) => void
  setFilters: (filters: Partial<ApartmentFilters>) => void
  resetFilters: () => void
  toggleDarkMode: () => void
  setSortBy: (sortBy: ApartmentState["sortBy"]) => void
}

const defaultLocation: ApartmentLocation = {
  address: "",
  city: "Unknown",
  district: "Unknown",
  country: "Kosovo",
  coordinates: [42.6629, 21.1655],
}

const getLocation = (apartment: Apartment): ApartmentLocation => {
  if (apartment.location) {
    return apartment.location
  }

  return {
    ...defaultLocation,
    address: apartment.address || defaultLocation.address,
    coordinates: [apartment.latitude ?? defaultLocation.coordinates[0], apartment.longitude ?? defaultLocation.coordinates[1]],
  }
}

const getArea = (apartment: Apartment): number => apartment.area ?? apartment.squareMeters ?? 0
const getFeatures = (apartment: Apartment): string[] => apartment.features ?? []

// Ray-casting algorithm to check if a point is inside a polygon
const isPointInPolygon = (point: [number, number], polygon: [number, number][]): boolean => {
  const [x, y] = point
  let inside = false

  for (let i = 0, j = polygon.length - 1; i < polygon.length; j = i++) {
    const [xi, yi] = polygon[i]
    const [xj, yj] = polygon[j]

    if (((yi > y) !== (yj > y)) && (x < (xj - xi) * (y - yi) / (yj - yi) + xi)) {
      inside = !inside
    }
  }

  return inside
}

const filterApartments = (apartments: Apartment[], filters: ApartmentFilters): Apartment[] => {
  return apartments.filter((apartment) => {
    const {
      priceMin,
      priceMax,
      minPrice,
      maxPrice,
      areaMin,
      areaMax,
      minArea,
      maxArea,
      bedrooms,
      bathrooms,
      forSale,
      forRent,
      district,
      search,
      polygonBounds,
    } = filters
    const resolvedPriceMin = priceMin ?? minPrice
    const resolvedPriceMax = priceMax ?? maxPrice
    const resolvedAreaMin = areaMin ?? minArea
    const resolvedAreaMax = areaMax ?? maxArea
    const location = getLocation(apartment)
    const area = getArea(apartment)
    const features = getFeatures(apartment)

    if (resolvedPriceMin !== undefined && apartment.price < resolvedPriceMin) return false
    if (resolvedPriceMax !== undefined && apartment.price > resolvedPriceMax) return false
    if (resolvedAreaMin !== undefined && area < resolvedAreaMin) return false
    if (resolvedAreaMax !== undefined && area > resolvedAreaMax) return false
    if (bedrooms !== undefined && apartment.bedrooms < bedrooms) return false
    if (bathrooms !== undefined && apartment.bathrooms < bathrooms) return false
    const listingType = apartment.listingType?.toString().toLowerCase()
    const isForSale = apartment.forSale ?? listingType === "sale"
    const isForRent = apartment.forRent ?? listingType === "rent"

    if (forSale !== undefined && forSale === true && isForSale !== forSale) return false
    if (forRent !== undefined && forRent === true && isForRent !== forRent) return false
    if (district !== undefined && district !== "" && location.district !== district) return false

    // Polygon bounds filter - check if apartment is inside the drawn polygon
    if (polygonBounds && polygonBounds.length >= 3) {
      const apartmentCoords: [number, number] = location.coordinates
      if (!isPointInPolygon(apartmentCoords, polygonBounds)) {
        return false
      }
    }

    if (search && search.trim() !== "") {
      const searchLower = search.toLowerCase().trim()
      const matchesTitle = (apartment.title ?? "").toLowerCase().includes(searchLower)
      const matchesDescription = (apartment.description ?? "").toLowerCase().includes(searchLower)
      const matchesAddress = location.address.toLowerCase().includes(searchLower)
      const matchesCity = location.city.toLowerCase().includes(searchLower)
      const matchesDistrict = location.district.toLowerCase().includes(searchLower)
      const matchesFeatures = features.some((feature) => feature.toLowerCase().includes(searchLower))

      if (
        !matchesTitle &&
        !matchesDescription &&
        !matchesAddress &&
        !matchesCity &&
        !matchesDistrict &&
        !matchesFeatures
      ) {
        return false
      }
    }

    return true
  })
}

const sortApartments = (apartments: Apartment[], sortBy: ApartmentState["sortBy"]): Apartment[] => {
  const sorted = [...apartments]

  switch (sortBy) {
    case "price-asc":
      return sorted.sort((a, b) => a.price - b.price)
    case "price-desc":
      return sorted.sort((a, b) => b.price - a.price)
    case "area-asc":
      return sorted.sort((a, b) => getArea(a) - getArea(b))
    case "area-desc":
      return sorted.sort((a, b) => getArea(b) - getArea(a))
    case "newest":
      return sorted.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
    case "oldest":
      return sorted.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime())
    default:
      return sorted
  }
}

const initialApartments = config.features.useMock ? mockApartments : []
const initialSortedApartments = sortApartments(initialApartments, "newest")

export const useApartmentStore = create<ApartmentState>((set, get) => ({
  apartments: initialApartments,
  filteredApartments: initialSortedApartments,
  selectedApartment: null,
  filters: {},
  isDarkMode: false,
  sortBy: "newest",

  setApartments: (apartments) => {
    const filtered = filterApartments(apartments, get().filters)
    const sorted = sortApartments(filtered, get().sortBy)
    set({ apartments, filteredApartments: sorted })
  },

  setSelectedApartment: (apartment) => set({ selectedApartment: apartment }),

  setFilters: (newFilters) => {
    const updatedFilters = { ...get().filters, ...newFilters }
    set({ filters: updatedFilters })

    // Apply filters and sorting
    const filtered = filterApartments(get().apartments, updatedFilters)
    const sorted = sortApartments(filtered, get().sortBy)

    set({ filteredApartments: sorted })
  },

  resetFilters: () => {
    const sorted = sortApartments(get().apartments, get().sortBy)
    set({ filters: {}, filteredApartments: sorted })
  },

  toggleDarkMode: () => set((state) => ({ isDarkMode: !state.isDarkMode })),

  setSortBy: (sortBy) => {
    set({ sortBy })
    const filtered = filterApartments(get().apartments, get().filters)
    const sorted = sortApartments(filtered, sortBy)
    set({ filteredApartments: sorted })
  },
}))
