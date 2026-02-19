"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { motion, AnimatePresence } from "framer-motion"
import { Search, MapPin, ChevronLeft, ChevronRight, ChevronDown, ChevronUp, X } from "lucide-react"
import { useApartmentStore } from "@/store/apartment-store"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Slider } from "@/components/ui/slider"
import { Switch } from "@/components/ui/switch"
import { Badge } from "@/components/ui/badge"
import { ApartmentListings } from "@/components/apartment-listings"
import { cn } from "@/lib/utils"

interface FloatingPanelProps {
  state: "expanded" | "collapsed" | "hidden"
  position: "left" | "bottom"
  onStateChange: (state: "expanded" | "collapsed" | "hidden") => void
}

export function FloatingPanel({ state, position, onStateChange }: FloatingPanelProps) {
  const { filters, setFilters, filteredApartments } = useApartmentStore()
  const [priceRange, setPriceRange] = useState<[number, number]>([0, 500000])
  const [searchQuery, setSearchQuery] = useState("")
  const [activeFilters, setActiveFilters] = useState<string[]>([])
  const [activeTab, setActiveTab] = useState<"search" | "listings">("listings")

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
    if (filters.forSale) newActiveFilters.push("For Sale")
    if (filters.forRent) newActiveFilters.push("For Rent")

    setActiveFilters(newActiveFilters)
  }, [filters])

  // Handle search query change
  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value)
  }

  // Handle search submit
  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    setFilters({ search: searchQuery })
    setActiveTab("listings")
  }

  // Handle price range change
  const handlePriceChange = (value: number[]) => {
    setPriceRange([value[0], value[1]])
    const newFilters = { priceMin: value[0], priceMax: value[1] }
    setFilters(newFilters)
  }

  // Handle district change
  const handleDistrictChange = (value: string) => {
    const newFilters = { district: value }
    setFilters(newFilters)
  }

  // Handle bedrooms change
  const handleBedroomsChange = (value: string) => {
    const newFilters = { bedrooms: Number.parseInt(value) }
    setFilters(newFilters)
  }

  // Handle bathrooms change
  const handleBathroomsChange = (value: string) => {
    const newFilters = { bathrooms: Number.parseInt(value) }
    setFilters(newFilters)
  }

  // Handle property type change
  const handleForSaleChange = (checked: boolean) => {
    const newFilters = { forSale: checked }
    setFilters(newFilters)
  }

  const handleForRentChange = (checked: boolean) => {
    const newFilters = { forRent: checked }
    setFilters(newFilters)
  }

  // Remove a filter
  const removeFilter = (filter: string) => {
    if (filter.includes("€")) {
      setFilters({ priceMin: undefined, priceMax: undefined })
      setPriceRange([0, 500000])
    } else if (filter.includes("BD")) {
      setFilters({ bedrooms: undefined })
    } else if (filter.includes("BA")) {
      setFilters({ bathrooms: undefined })
    } else if (filter === "For Sale") {
      setFilters({ forSale: false })
    } else if (filter === "For Rent") {
      setFilters({ forRent: false })
    } else {
      // Assume it's a district
      setFilters({ district: undefined })
    }
  }

  // Reset all filters
  const resetAllFilters = () => {
    setFilters({})
    setPriceRange([0, 500000])
    setSearchQuery("")
  }

  // Toggle panel state
  const togglePanel = () => {
    if (state === "expanded") {
      onStateChange("collapsed")
    } else {
      onStateChange("expanded")
    }
  }

  // Districts data
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

  // Panel variants for animations
  const panelVariants = {
    hidden: {
      y: position === "bottom" ? "100%" : 0,
      x: position === "left" ? "-100%" : 0,
      opacity: 0,
    },
    collapsed: {
      y: position === "bottom" ? "calc(100% - 80px)" : 0,
      x: position === "left" ? "calc(-100% + 80px)" : 0,
      opacity: 1,
    },
    expanded: {
      y: 0,
      x: 0,
      opacity: 1,
    },
  }

  // Don't render if state is hidden
  if (state === "hidden") {
    return null
  }

  return (
    <AnimatePresence>
      <motion.div
        className={cn(
          "absolute z-30 bg-card/90 dark:bg-card/95 backdrop-blur-md rounded-t-2xl shadow-lg overflow-hidden border",
          position === "bottom"
            ? "left-4 right-4 bottom-0 max-h-[80vh]"
            : "left-0 bottom-0 top-0 lg:w-[450px] lg:rounded-r-2xl lg:rounded-tl-none",
        )}
        initial="hidden"
        animate={state}
        variants={panelVariants}
        transition={{ type: "spring", stiffness: 300, damping: 30 }}
      >
        {/* Collapsed state header */}
        {state === "collapsed" && (
          <div className="p-4 flex items-center justify-between">
            <div className="flex items-center">
              <MapPin className="h-5 w-5 text-primary mr-2" />
              <span className="font-medium">{filteredApartments.length} properties found</span>
            </div>
            <Button variant="ghost" size="sm" onClick={() => onStateChange("expanded")}>
              {position === "bottom" ? <ChevronUp className="h-4 w-4" /> : <ChevronRight className="h-4 w-4" />}
            </Button>
          </div>
        )}

        {/* Expanded state content */}
        {state === "expanded" && (
          <div className="h-full flex flex-col">
            <div className="p-4 border-b flex items-center justify-between">
              <h2 className="text-lg font-semibold">Find Your Home</h2>
              <div className="flex items-center gap-2">
                <Button variant="ghost" size="sm" className="h-8 w-8 p-0" onClick={togglePanel}>
                  {position === "bottom" ? <ChevronDown className="h-4 w-4" /> : <ChevronLeft className="h-4 w-4" />}
                </Button>
                <Button variant="ghost" size="sm" className="h-8 w-8 p-0" onClick={() => onStateChange("hidden")}>
                  <X className="h-4 w-4" />
                </Button>
              </div>
            </div>

            <Tabs defaultValue="listings" value={activeTab} onValueChange={(value) => setActiveTab(value as any)}>
              <div className="px-4 pt-4 border-b">
                <TabsList className="grid grid-cols-2 w-full bg-gray-100 dark:bg-gray-800 rounded-xl p-1">
                  <TabsTrigger
                    value="listings"
                    className="rounded-lg data-[state=active]:bg-white data-[state=active]:shadow-sm data-[state=inactive]:text-gray-600 dark:data-[state=active]:bg-gray-700 dark:data-[state=inactive]:text-gray-400 transition-all duration-200"
                  >
                    <MapPin className="mr-2 h-4 w-4" />
                    Listings
                  </TabsTrigger>
                  <TabsTrigger
                    value="search"
                    className="rounded-lg data-[state=active]:bg-white data-[state=active]:shadow-sm data-[state=inactive]:text-gray-600 dark:data-[state=active]:bg-gray-700 dark:data-[state=inactive]:text-gray-400 transition-all duration-200"
                  >
                    <Search className="mr-2 h-4 w-4" />
                    Search
                  </TabsTrigger>
                </TabsList>
              </div>

              <div className="flex-1 overflow-y-auto">
                <TabsContent value="listings" className="m-0 p-0 h-full">
                  <ApartmentListings />
                </TabsContent>

                <TabsContent value="search" className="m-0 p-4 space-y-6">
                  <form onSubmit={handleSearchSubmit} className="relative">
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-muted-foreground" />
                    <Input
                      placeholder="Search by location, district..."
                      className="pl-10 h-12"
                      value={searchQuery}
                      onChange={handleSearchChange}
                    />
                    <Button type="submit" className="absolute right-1 top-1 h-10">
                      Search
                    </Button>
                  </form>

                  <div className="space-y-4">
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

                    <div className="space-y-4">
                      <div className="flex items-center justify-between">
                        <Label>Price Range</Label>
                        <div className="text-sm text-muted-foreground">
                          {priceRange[0].toLocaleString()} € - {priceRange[1].toLocaleString()} €
                        </div>
                      </div>
                      <Slider
                        defaultValue={[0, 500000]}
                        max={500000}
                        step={5000}
                        value={[priceRange[0], priceRange[1]]}
                        onValueChange={handlePriceChange}
                        className="py-4 range-slider"
                      />
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

                    {/* Active filters */}
                    {activeFilters.length > 0 && (
                      <div className="mt-6">
                        <div className="flex items-center justify-between mb-2">
                          <Label>Active Filters</Label>
                          <Button variant="ghost" size="sm" onClick={resetAllFilters}>
                            Clear All
                          </Button>
                        </div>
                        <div className="flex flex-wrap gap-2">
                          {activeFilters.map((filter) => (
                            <Badge key={filter} variant="secondary" className="flex items-center gap-1">
                              {filter}
                              <Button
                                variant="ghost"
                                size="sm"
                                className="h-4 w-4 p-0 ml-1"
                                onClick={() => removeFilter(filter)}
                              >
                                <X className="h-3 w-3" />
                                <span className="sr-only">Remove {filter} filter</span>
                              </Button>
                            </Badge>
                          ))}
                        </div>
                      </div>
                    )}
                  </div>

                  <div className="pt-4">
                    <Button className="w-full" onClick={() => setActiveTab("listings")}>
                      View {filteredApartments.length} Properties
                    </Button>
                  </div>
                </TabsContent>
              </div>
            </Tabs>

            <div className="p-4 border-t bg-background/50">
              <div className="flex items-center justify-between">
                <div className="flex items-center">
                  <MapPin className="h-5 w-5 text-primary mr-2" />
                  <span className="font-medium">{filteredApartments.length} properties found</span>
                </div>
                <Button size="sm" onClick={() => onStateChange("hidden")}>
                  View Map
                </Button>
              </div>
            </div>
          </div>
        )}
      </motion.div>
    </AnimatePresence>
  )
}
