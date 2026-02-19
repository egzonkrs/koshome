"use client"

import { useState, useEffect } from "react"
import { useSearchParams } from "next/navigation"
import { Header } from "@/components/header"
import { ApartmentsGrid } from "@/components/apartments/apartments-grid"
import { ApartmentsFilters } from "@/components/apartments/apartments-filters"
import { ApartmentsSorting } from "@/components/apartments/apartments-sorting"
import { ApartmentsPagination } from "@/components/apartments/apartments-pagination"
import { useApartmentStore } from "@/store/apartment-store"
import { useApartments } from "@/lib/query"
import { Button } from "@/components/ui/button"
import { MapIcon, GridIcon } from "lucide-react"
import dynamic from "next/dynamic"

// Dynamically import the map component with SSR disabled
const ApartmentsMap = dynamic(() => import("@/components/apartments/apartments-map"), {
  ssr: false,
  loading: () => <div className="h-[calc(100vh-250px)] rounded-lg bg-muted animate-pulse" />,
})

export default function ApartmentsPage() {
  const [mounted, setMounted] = useState(false)
  const [viewMode, setViewMode] = useState<"grid" | "map">("grid")
  const [currentPage, setCurrentPage] = useState(1)
  const { filteredApartments, filters, setApartments } = useApartmentStore()
  const searchParams = useSearchParams()
  const { data } = useApartments(filters)

  const ITEMS_PER_PAGE = 9
  const totalPages = Math.ceil(filteredApartments.length / ITEMS_PER_PAGE)

  // Get current apartments for pagination
  const indexOfLastApartment = currentPage * ITEMS_PER_PAGE
  const indexOfFirstApartment = indexOfLastApartment - ITEMS_PER_PAGE
  const currentApartments = filteredApartments.slice(indexOfFirstApartment, indexOfLastApartment)

  // Fix hydration issues
  useEffect(() => {
    setMounted(true)

    // Set page from URL if available
    const page = searchParams.get("page")
    if (page) {
      setCurrentPage(Number.parseInt(page))
    }
  }, [searchParams])

  useEffect(() => {
    const items = data?.data?.items
    if (data?.isSuccess && items) {
      setApartments(items)
    }
  }, [data, setApartments])

  useEffect(() => {
    if (currentPage > totalPages && totalPages > 0) {
      setCurrentPage(1)
    }
  }, [totalPages, currentPage])

  if (!mounted) return null

  return (
    <div className="flex flex-col min-h-screen bg-background">
      <Header />

      <main className="flex-1 container mx-auto px-4 py-8">
        <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4 bg-background text-foreground">
          <div>
            <h1 className="text-3xl font-bold">Apartments</h1>
            <p className="text-muted-foreground">{filteredApartments.length} properties available</p>
          </div>

          <div className="flex items-center gap-4">
            <ApartmentsSorting />

            <div className="flex bg-muted rounded-md p-1">
              <Button
                variant="ghost"
                size="sm"
                className={`rounded-md ${viewMode === "grid" ? "bg-background" : ""}`}
                onClick={() => setViewMode("grid")}
              >
                <GridIcon className="h-4 w-4 mr-2" />
                Grid
              </Button>
              <Button
                variant="ghost"
                size="sm"
                className={`rounded-md ${viewMode === "map" ? "bg-background" : ""}`}
                onClick={() => setViewMode("map")}
              >
                <MapIcon className="h-4 w-4 mr-2" />
                Map
              </Button>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-[300px_1fr] gap-8">
          <aside className="bg-card p-6 rounded-lg shadow-sm lg:sticky lg:top-24 h-fit border">
            <ApartmentsFilters />
          </aside>

          <div className="space-y-6">
            {viewMode === "grid" ? (
              <>
                <ApartmentsGrid apartments={currentApartments} />

                {totalPages > 1 && (
                  <ApartmentsPagination
                    currentPage={currentPage}
                    totalPages={totalPages}
                    onPageChange={setCurrentPage}
                  />
                )}
              </>
            ) : (
              <div className="h-[calc(100vh-250px)] rounded-lg overflow-hidden">
                {mounted && <ApartmentsMap apartments={filteredApartments} />}
              </div>
            )}
          </div>
        </div>
      </main>
    </div>
  )
}
