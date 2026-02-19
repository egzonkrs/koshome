"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { useRouter, usePathname, useSearchParams } from "next/navigation"
import { useApartmentStore } from "@/store/apartment-store"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Slider } from "@/components/ui/slider"
import { Switch } from "@/components/ui/switch"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Badge } from "@/components/ui/badge"
import { X, Search } from "lucide-react"

export function ApartmentsFilters() {
  const { filters, setFilters, resetFilters } = useApartmentStore()
  const [priceRange, setPriceRange] = useState<[number, number]>([0, 500000])
  const [areaRange, setAreaRange] = useState<[number, number]>([0, 200])
  const [searchQuery, setSearchQuery] = useState("")
  const [activeFilters, setActiveFilters] = useState<string[]>([])

  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  // Initialize filters from URL on component mount
  useEffect(() => {
    const urlParams = new URLSearchParams(searchParams.toString())
    const filtersFromUrl: Record<string, any> = {}

    // Parse price range
    if (urlParams.has("priceMin") && urlParams.has("priceMax")) {
      const min = Number.parseInt(urlParams.get("priceMin") || "0")
      const max = Number.parseInt(urlParams.get("priceMax") || "500000")
      setPriceRange([min, max])
      filtersFromUrl.priceMin = min
      filtersFromUrl.priceMax = max
    }

    // Parse area range
    if (urlParams.has("areaMin") && urlParams.has("areaMax")) {
      const min = Number.parseInt(urlParams.get("areaMin") || "0")
      const max = Number.parseInt(urlParams.get("areaMax") || "200")
      setAreaRange([min, max])
      filtersFromUrl.areaMin = min
      filtersFromUrl.areaMax = max
    }

    // Parse district
    if (urlParams.has("district")) {
      filtersFromUrl.district = urlParams.get("district")
    }

    // Parse bedrooms
    if (urlParams.has("bedrooms")) {
      filtersFromUrl.bedrooms = Number.parseInt(urlParams.get("bedrooms") || "0")
    }

    // Parse bathrooms
    if (urlParams.has("bathrooms")) {
      filtersFromUrl.bathrooms = Number.parseInt(urlParams.get("bathrooms") || "0")
    }

    // Parse forSale and forRent
    if (urlParams.has("forSale")) {
      filtersFromUrl.forSale = urlParams.get("forSale") === "true"
    }

    if (urlParams.has("forRent")) {
      filtersFromUrl.forRent = urlParams.get("forRent") === "true"
    }

    // Parse search query
    if (urlParams.has("search")) {
      setSearchQuery(urlParams.get("search") || "")
    }

    // Apply filters from URL
    if (Object.keys(filtersFromUrl).length > 0) {
      setFilters(filtersFromUrl)
    }
  }, [searchParams, setFilters])

  // Update active filters when filters change
  useEffect(() => {
    const newActiveFilters: string[] = []

    if (filters.district) newActiveFilters.push(filters.district)
    if (filters.bedrooms) newActiveFilters.push(`${filters.bedrooms}+ BD`)
    if (filters.bathrooms) newActiveFilters.push(`${filters.bathrooms}+ BA`)
    if (filters.priceMin || filters.priceMax) {
      newActiveFilters.push(
        `${filters.priceMin?.toLocaleString() || 0}€ - ${filters.priceMax?.toLocaleString() || "∞"}€`,
      )
    }
    if (filters.areaMin || filters.areaMax) {
      newActiveFilters.push(`${filters.areaMin || 0} - ${filters.areaMax || 200} m²`)
    }
    if (filters.forSale) newActiveFilters.push("For Sale")
    if (filters.forRent) newActiveFilters.push("For Rent")

    setActiveFilters(newActiveFilters)
  }, [filters])

  // Update URL when filters change
  const updateUrl = (newFilters: Record<string, any>) => {
    const urlParams = new URLSearchParams(searchParams.toString())

    // Update or remove price range params
    if (newFilters.priceMin !== undefined) {
      urlParams.set("priceMin", newFilters.priceMin.toString())
    }
    if (newFilters.priceMax !== undefined) {
      urlParams.set("priceMax", newFilters.priceMax.toString())
    }

    // Update or remove area range params
    if (newFilters.areaMin !== undefined) {
      urlParams.set("areaMin", newFilters.areaMin.toString())
    }
    if (newFilters.areaMax !== undefined) {
      urlParams.set("areaMax", newFilters.areaMax.toString())
    }

    // Update or remove district param
    if (newFilters.district !== undefined) {
      if (newFilters.district) {
        urlParams.set("district", newFilters.district)
      } else {
        urlParams.delete("district")
      }
    }

    // Update or remove bedrooms param
    if (newFilters.bedrooms !== undefined) {
      if (newFilters.bedrooms) {
        urlParams.set("bedrooms", newFilters.bedrooms.toString())
      } else {
        urlParams.delete("bedrooms")
      }
    }

    // Update or remove bathrooms param
    if (newFilters.bathrooms !== undefined) {
      if (newFilters.bathrooms) {
        urlParams.set("bathrooms", newFilters.bathrooms.toString())
      } else {
        urlParams.delete("bathrooms")
      }
    }

    // Update or remove forSale param
    if (newFilters.forSale !== undefined) {
      urlParams.set("forSale", newFilters.forSale.toString())
    }

    // Update or remove forRent param
    if (newFilters.forRent !== undefined) {
      urlParams.set("forRent", newFilters.forRent.toString())
    }

    // Update or remove search param
    if (newFilters.search !== undefined) {
      if (newFilters.search) {
        urlParams.set("search", newFilters.search)
      } else {
        urlParams.delete("search")
      }
    }

    // Reset to page 1 when filters change
    urlParams.set("page", "1")

    // Update URL without refreshing the page
    router.push(`${pathname}?${urlParams.toString()}`, { scroll: false })
  }

  const handlePriceChange = (value: number[]) => {
    setPriceRange([value[0], value[1]])
    const newFilters = { priceMin: value[0], priceMax: value[1] }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleAreaChange = (value: number[]) => {
    setAreaRange([value[0], value[1]])
    const newFilters = { areaMin: value[0], areaMax: value[1] }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleDistrictChange = (value: string) => {
    const newFilters = { district: value }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleBedroomsChange = (value: string) => {
    const newFilters = { bedrooms: Number.parseInt(value) }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleBathroomsChange = (value: string) => {
    const newFilters = { bathrooms: Number.parseInt(value) }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleForSaleChange = (checked: boolean) => {
    const newFilters = { forSale: checked }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleForRentChange = (checked: boolean) => {
    const newFilters = { forRent: checked }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value)
  }

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    const newFilters = { search: searchQuery }
    setFilters(newFilters)
    updateUrl(newFilters)
  }

  // Remove a filter
  const removeFilter = (filter: string) => {
    if (filter.includes("€")) {
      setFilters({ priceMin: undefined, priceMax: undefined })
      setPriceRange([0, 500000])
      updateUrl({ priceMin: undefined, priceMax: undefined })
    } else if (filter.includes("BD")) {
      setFilters({ bedrooms: undefined })
      updateUrl({ bedrooms: undefined })
    } else if (filter.includes("BA")) {
      setFilters({ bathrooms: undefined })
      updateUrl({ bathrooms: undefined })
    } else if (filter === "For Sale") {
      setFilters({ forSale: false })
      updateUrl({ forSale: false })
    } else if (filter === "For Rent") {
      setFilters({ forRent: false })
      updateUrl({ forRent: false })
    } else if (filter.includes("m²")) {
      setFilters({ areaMin: undefined, areaMax: undefined })
      setAreaRange([0, 200])
      updateUrl({ areaMin: undefined, areaMax: undefined })
    } else {
      // Assume it's a district
      setFilters({ district: undefined })
      updateUrl({ district: undefined })
    }
  }

  const handleResetFilters = () => {
    resetFilters()
    setPriceRange([0, 500000])
    setAreaRange([0, 200])
    setSearchQuery("")
    router.push(pathname, { scroll: false })
  }

  const districts = [
    "City Center",
    "Arbëri",
    "Dardania",
    "Kalabria",
    "Old Town",
    "Ulpiana",
    "Veternik",
    "Bregu i Diellit",
  ]

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-lg font-semibold mb-4">Filters</h2>

        <form onSubmit={handleSearchSubmit} className="relative mb-6">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search by location, district..."
            className="pl-9"
            value={searchQuery}
            onChange={handleSearchChange}
          />
        </form>
      </div>

      {/* Active filters */}
      {activeFilters.length > 0 && (
        <div>
          <div className="flex items-center justify-between mb-2">
            <Label>Active Filters</Label>
            <Button variant="ghost" size="sm" onClick={handleResetFilters} className="h-8 px-2 text-xs">
              Clear All
            </Button>
          </div>
          <div className="flex flex-wrap gap-2">
            {activeFilters.map((filter) => (
              <Badge key={filter} variant="secondary" className="flex items-center gap-1">
                {filter}
                <Button variant="ghost" size="sm" className="h-4 w-4 p-0 ml-1" onClick={() => removeFilter(filter)}>
                  <X className="h-3 w-3" />
                  <span className="sr-only">Remove {filter} filter</span>
                </Button>
              </Badge>
            ))}
          </div>
        </div>
      )}

      <div className="space-y-6">
        <div className="space-y-4">
          <Label>Price Range</Label>
          <Slider
            defaultValue={[0, 500000]}
            max={500000}
            step={5000}
            value={[priceRange[0], priceRange[1]]}
            onValueChange={handlePriceChange}
            className="py-4 range-slider"
          />
          <div className="flex items-center justify-between text-sm text-muted-foreground">
            <span>{priceRange[0].toLocaleString()} €</span>
            <span>{priceRange[1].toLocaleString()} €</span>
          </div>
        </div>

        <div className="space-y-4">
          <Label>Area</Label>
          <Slider
            defaultValue={[0, 200]}
            max={200}
            step={5}
            value={[areaRange[0], areaRange[1]]}
            onValueChange={handleAreaChange}
            className="py-4 range-slider"
          />
          <div className="flex items-center justify-between text-sm text-muted-foreground">
            <span>{areaRange[0]} m²</span>
            <span>{areaRange[1]} m²</span>
          </div>
        </div>

        <div className="space-y-2">
          <Label htmlFor="district">District</Label>
          <Select onValueChange={handleDistrictChange} value={filters.district}>
            <SelectTrigger id="district">
              <SelectValue placeholder="Any district" />
            </SelectTrigger>
            <SelectContent>
              {districts.map((district) => (
                <SelectItem key={district} value={district}>
                  {district}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div className="space-y-2">
            <Label htmlFor="bedrooms">Bedrooms</Label>
            <Select onValueChange={handleBedroomsChange} value={filters.bedrooms?.toString()}>
              <SelectTrigger id="bedrooms">
                <SelectValue placeholder="Any" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="0">Studio</SelectItem>
                <SelectItem value="1">1+</SelectItem>
                <SelectItem value="2">2+</SelectItem>
                <SelectItem value="3">3+</SelectItem>
                <SelectItem value="4">4+</SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="bathrooms">Bathrooms</Label>
            <Select onValueChange={handleBathroomsChange} value={filters.bathrooms?.toString()}>
              <SelectTrigger id="bathrooms">
                <SelectValue placeholder="Any" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="1">1+</SelectItem>
                <SelectItem value="2">2+</SelectItem>
                <SelectItem value="3">3+</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>

        <div className="space-y-2">
          <Label>Property Type</Label>
          <div className="grid grid-cols-2 gap-4 pt-2">
            <div className="flex items-center gap-2">
              <Switch id="sale-switch" checked={filters.forSale} onCheckedChange={handleForSaleChange} />
              <Label htmlFor="sale-switch" className="text-sm cursor-pointer">
                For Sale
              </Label>
            </div>
            <div className="flex items-center gap-2">
              <Switch id="rent-switch" checked={filters.forRent} onCheckedChange={handleForRentChange} />
              <Label htmlFor="rent-switch" className="text-sm cursor-pointer">
                For Rent
              </Label>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
