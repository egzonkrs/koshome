"use client"

import { useState, useEffect } from "react"
import { X, ChevronLeft, ChevronRight } from "lucide-react"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"

interface ImageZoomModalProps {
  isOpen: boolean
  onClose: () => void
  images: string[]
  initialIndex?: number
  title: string
}

export function ImageZoomModal({ isOpen, onClose, images, initialIndex = 0, title }: ImageZoomModalProps) {
  const [currentIndex, setCurrentIndex] = useState(initialIndex)
  const [isLoading, setIsLoading] = useState(true)

  // Reset current index when modal opens or initialIndex changes
  useEffect(() => {
    if (isOpen) {
      setCurrentIndex(initialIndex)
    }
  }, [isOpen, initialIndex])

  // Handle keyboard navigation
  useEffect(() => {
    if (!isOpen) return

    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === "Escape") {
        onClose()
      } else if (e.key === "ArrowLeft") {
        prevImage()
      } else if (e.key === "ArrowRight") {
        nextImage()
      }
    }

    window.addEventListener("keydown", handleKeyDown)
    return () => window.removeEventListener("keydown", handleKeyDown)
  }, [isOpen, onClose])

  // Prevent body scrolling when modal is open
  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = "hidden"
    } else {
      document.body.style.overflow = ""
    }
    return () => {
      document.body.style.overflow = ""
    }
  }, [isOpen])

  // Preload adjacent images
  useEffect(() => {
    if (!isOpen || images.length <= 1) return

    const nextIdx = currentIndex === images.length - 1 ? 0 : currentIndex + 1
    const prevIdx = currentIndex === 0 ? images.length - 1 : currentIndex - 1

    const preloadNext = new Image()
    preloadNext.src = images[nextIdx]

    const preloadPrev = new Image()
    preloadPrev.src = images[prevIdx]
  }, [currentIndex, images, isOpen])

  const nextImage = () => {
    setCurrentIndex((prev) => (prev === images.length - 1 ? 0 : prev + 1))
    setIsLoading(true)
  }

  const prevImage = () => {
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1))
    setIsLoading(true)
  }

  const handleThumbnailClick = (index: number) => {
    if (index !== currentIndex) {
      setCurrentIndex(index)
      setIsLoading(true)
    }
  }

  if (!isOpen) return null

  return (
    <div
      className="fixed inset-0 z-50 bg-background/80 backdrop-blur-sm flex items-center justify-center"
      onClick={onClose}
    >
      <div className="relative w-full h-full flex flex-col md:flex-row" onClick={(e) => e.stopPropagation()}>
        {/* Close button */}
        <Button
          variant="ghost"
          size="icon"
          className="absolute top-4 right-4 z-50 rounded-full bg-background/50 hover:bg-background/80"
          onClick={onClose}
        >
          <X className="h-6 w-6" />
          <span className="sr-only">Close</span>
        </Button>

        {/* Image counter */}
        <div className="absolute top-4 left-4 z-50 bg-background/50 backdrop-blur-sm px-3 py-1.5 rounded-md text-sm font-medium">
          {currentIndex + 1} / {images.length}
        </div>

        {/* Thumbnails sidebar - visible on larger screens */}
        <div className="hidden md:flex flex-col w-24 h-full bg-background/50 backdrop-blur-sm p-2 gap-2 overflow-y-auto">
          {images.map((image, index) => (
            <div
              key={index}
              className={cn(
                "aspect-square rounded-md overflow-hidden cursor-pointer border-2 transition-all",
                index === currentIndex ? "border-primary" : "border-transparent hover:border-primary/50",
              )}
              onClick={() => handleThumbnailClick(index)}
            >
              <img
                src={image || "/placeholder.svg"}
                alt={`${title} - Thumbnail ${index + 1}`}
                className="w-full h-full object-cover"
                loading="lazy"
              />
            </div>
          ))}
        </div>

        {/* Main image container */}
        <div
          className="flex-1 flex items-center justify-center p-4 md:p-8 relative"
          onClick={(e) => e.stopPropagation()}
        >
          {isLoading && (
            <div className="absolute inset-0 flex items-center justify-center bg-background/20">
              <div className="w-8 h-8 border-4 border-primary border-t-transparent rounded-full animate-spin"></div>
            </div>
          )}
          <img
            src={images[currentIndex] || "/placeholder.svg"}
            alt={`${title} - Image ${currentIndex + 1}`}
            className="max-w-full max-h-full object-contain"
            onLoad={() => setIsLoading(false)}
          />

          {/* Navigation buttons */}
          <Button
            variant="ghost"
            size="icon"
            className="absolute left-4 top-1/2 -translate-y-1/2 rounded-full bg-background/50 hover:bg-background/80"
            onClick={prevImage}
          >
            <ChevronLeft className="h-6 w-6" />
            <span className="sr-only">Previous image</span>
          </Button>

          <Button
            variant="ghost"
            size="icon"
            className="absolute right-4 top-1/2 -translate-y-1/2 rounded-full bg-background/50 hover:bg-background/80"
            onClick={nextImage}
          >
            <ChevronRight className="h-6 w-6" />
            <span className="sr-only">Next image</span>
          </Button>
        </div>

        {/* Mobile thumbnails - visible at bottom on small screens */}
        <div
          className="md:hidden flex overflow-x-auto gap-2 p-2 bg-background/50 backdrop-blur-sm"
          onClick={(e) => e.stopPropagation()}
        >
          {images.map((image, index) => (
            <div
              key={index}
              className={cn(
                "flex-shrink-0 w-16 h-16 rounded-md overflow-hidden cursor-pointer border-2 transition-all",
                index === currentIndex ? "border-primary" : "border-transparent hover:border-primary/50",
              )}
              onClick={() => handleThumbnailClick(index)}
            >
              <img
                src={image || "/placeholder.svg"}
                alt={`${title} - Thumbnail ${index + 1}`}
                className="w-full h-full object-cover"
                loading="lazy"
              />
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
