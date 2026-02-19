"use client"

import { useRouter, usePathname, useSearchParams } from "next/navigation"
import { Button } from "@/components/ui/button"
import { ChevronLeft, ChevronRight, MoreHorizontal } from "lucide-react"

interface ApartmentsPaginationProps {
  currentPage: number
  totalPages: number
  onPageChange: (page: number) => void
}

export function ApartmentsPagination({ currentPage, totalPages, onPageChange }: ApartmentsPaginationProps) {
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  // Update URL when page changes
  const handlePageChange = (page: number) => {
    onPageChange(page)

    // Update URL with new page
    const params = new URLSearchParams(searchParams.toString())
    params.set("page", page.toString())
    router.push(`${pathname}?${params.toString()}`, { scroll: false })
  }

  // Generate page numbers to display
  const getPageNumbers = () => {
    const pageNumbers = []

    if (totalPages <= 7) {
      // If 7 or fewer pages, show all
      for (let i = 1; i <= totalPages; i++) {
        pageNumbers.push(i)
      }
    } else {
      // Always show first page
      pageNumbers.push(1)

      // Show dots if not on first 3 pages
      if (currentPage > 3) {
        pageNumbers.push("dots1")
      }

      // Determine start and end of middle pages
      let startPage = Math.max(2, currentPage - 1)
      let endPage = Math.min(totalPages - 1, currentPage + 1)

      // Adjust if at start or end
      if (currentPage <= 3) {
        endPage = 4
      }

      if (currentPage >= totalPages - 2) {
        startPage = totalPages - 3
      }

      // Add middle pages
      for (let i = startPage; i <= endPage; i++) {
        pageNumbers.push(i)
      }

      // Show dots if not on last 3 pages
      if (currentPage < totalPages - 2) {
        pageNumbers.push("dots2")
      }

      // Always show last page
      pageNumbers.push(totalPages)
    }

    return pageNumbers
  }

  return (
    <div className="flex justify-center items-center mt-8">
      <div className="flex items-center gap-1">
        <Button
          variant="outline"
          size="icon"
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
          className="h-9 w-9"
        >
          <ChevronLeft className="h-4 w-4" />
          <span className="sr-only">Previous page</span>
        </Button>

        {getPageNumbers().map((page, index) => {
          if (page === "dots1" || page === "dots2") {
            return (
              <Button key={`dots-${index}`} variant="ghost" disabled className="h-9 w-9">
                <MoreHorizontal className="h-4 w-4" />
                <span className="sr-only">More pages</span>
              </Button>
            )
          }

          return (
            <Button
              key={page}
              variant={currentPage === page ? "default" : "outline"}
              onClick={() => handlePageChange(page as number)}
              className="h-9 w-9"
            >
              {page}
            </Button>
          )
        })}

        <Button
          variant="outline"
          size="icon"
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
          className="h-9 w-9"
        >
          <ChevronRight className="h-4 w-4" />
          <span className="sr-only">Next page</span>
        </Button>
      </div>
    </div>
  )
}
