"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { useApartmentStore } from "@/store/apartment-store"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Slider } from "@/components/ui/slider"
import { Switch } from "@/components/ui/switch"
import { Search, SlidersHorizontal, X, Moon, Sun } from "lucide-react"
import { Sheet, SheetContent, SheetHeader, SheetTitle, SheetTrigger } from "@/components/ui/sheet"
import { useTheme } from "next-themes"

export function FilterBar() {
  const { filters, setFilters, resetFilters } = useApartmentStore()
  const [priceRange, setPriceRange] = useState<[number, number]>([0, 500000])
  const [areaRange, setAreaRange] = useState<[number, number]>([0, 200])
  const [searchQuery, setSearchQuery] = useState("")
  const [mounted, setMounted] = useState(false)
  const router = useRouter()
  const searchParams = useSearchParams()
  const { resolvedTheme, setTheme } = useTheme()

  useEffect(() => {
    setMounted(true)
  }, [])

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

    // Update URL without refreshing the page
    const newUrl = `${window.location.pathname}?${urlParams.toString()}`
    router.push(newUrl, { scroll: false })
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

  const handleResetFilters = () => {
    resetFilters()
    setPriceRange([0, 500000])
    setAreaRange([0, 200])
    setSearchQuery("")
    router.push(window.location.pathname, { scroll: false })
  }

  const toggleTheme = () => {
    setTheme(resolvedTheme === "dark" ? "light" : "dark")
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
    <div className="bg-background border-b p-4 relative z-30">
      <div className="flex flex-wrap items-center gap-4">
        <form onSubmit={handleSearchSubmit} className="relative flex-1 min-w-[200px]">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search by location, district..."
            className="pl-9"
            value={searchQuery}
            onChange={handleSearchChange}
          />
        </form>

        <Select onValueChange={handleDistrictChange} value={filters.district}>
          <SelectTrigger className="w-[180px]">
            <SelectValue placeholder="District" />
          </SelectTrigger>
          <SelectContent className="z-50">
            {districts.map((district) => (
              <SelectItem key={district} value={district}>
                {district}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        <Select onValueChange={handleBedroomsChange} value={filters.bedrooms?.toString()}>
          <SelectTrigger className="w-[150px]">
            <SelectValue placeholder="Bedrooms" />
          </SelectTrigger>
          <SelectContent className="z-50">
            <SelectItem value="0">Studio</SelectItem>
            <SelectItem value="1">1+ Bedroom</SelectItem>
            <SelectItem value="2">2+ Bedrooms</SelectItem>
            <SelectItem value="3">3+ Bedrooms</SelectItem>
            <SelectItem value="4">4+ Bedrooms</SelectItem>
          </SelectContent>
        </Select>

        <div className="flex items-center gap-2">
          <Label htmlFor="sale-switch" className="text-sm">
            For Sale
          </Label>
          <Switch id="sale-switch" checked={filters.forSale} onCheckedChange={handleForSaleChange} />
        </div>

        <div className="flex items-center gap-2">
          <Label htmlFor="rent-switch" className="text-sm">
            For Rent
          </Label>
          <Switch id="rent-switch" checked={filters.forRent} onCheckedChange={handleForRentChange} />
        </div>

        <Sheet>
          <SheetTrigger asChild>
            <Button variant="outline" size="icon">
              <SlidersHorizontal className="h-4 w-4" />
              <span className="sr-only">More filters</span>
            </Button>
          </SheetTrigger>
          <SheetContent>
            <SheetHeader>
              <SheetTitle>Filters</SheetTitle>
            </SheetHeader>
            <div className="py-6 space-y-6">
              <div className="space-y-4">
                <h3 className="font-medium">Price Range</h3>
                <Slider
                  defaultValue={[0, 500000]}
                  max={500000}
                  step={5000}
                  value={[priceRange[0], priceRange[1]]}
                  onValueChange={handlePriceChange}
                />
                <div className="flex items-center justify-between">
                  <span>{priceRange[0].toLocaleString()} €</span>
                  <span>{priceRange[1].toLocaleString()} €</span>
                </div>
              </div>

              <div className="space-y-4">
                <h3 className="font-medium">Area</h3>
                <Slider
                  defaultValue={[0, 200]}
                  max={200}
                  step={5}
                  value={[areaRange[0], areaRange[1]]}
                  onValueChange={handleAreaChange}
                />
                <div className="flex items-center justify-between">
                  <span>{areaRange[0]} m²</span>
                  <span>{areaRange[1]} m²</span>
                </div>
              </div>

              <div className="space-y-4">
                <h3 className="font-medium">Bathrooms</h3>
                <Select onValueChange={handleBathroomsChange} value={filters.bathrooms?.toString()}>
                  <SelectTrigger>
                    <SelectValue placeholder="Any" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="1">1+ Bathroom</SelectItem>
                    <SelectItem value="2">2+ Bathrooms</SelectItem>
                    <SelectItem value="3">3+ Bathrooms</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="mt-6">
              <Button variant="outline" className="w-full" onClick={handleResetFilters}>
                <X className="mr-2 h-4 w-4" />
                Reset Filters
              </Button>
            </div>
          </SheetContent>
        </Sheet>

        {mounted && (
          <Button variant="outline" size="icon" onClick={toggleTheme}>
            {resolvedTheme === "dark" ? <Sun className="h-4 w-4" /> : <Moon className="h-4 w-4" />}
            <span className="sr-only">Toggle theme</span>
          </Button>
        )}
      </div>
    </div>
  )
}
