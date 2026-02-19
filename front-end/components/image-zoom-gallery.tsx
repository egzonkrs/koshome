"use client"

import type React from "react"

import { useState } from "react"
import { ChevronLeft, ChevronRight, ZoomIn } from "lucide-react"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"
import { ImageZoomModal } from "./image-zoom-modal"

interface ImageZoomGalleryProps {
  images: string[]
  title: string
}

export function ImageZoomGallery({ images, title }: ImageZoomGalleryProps) {
  const [currentIndex, setCurrentIndex] = useState(0)
  const [modalOpen, setModalOpen] = useState(false)
  const [touchStart, setTouchStart] = useState<number | null>(null)
  const [touchEnd, setTouchEnd] = useState<number | null>(null)

  // Minimum swipe distance (in px)
  const minSwipeDistance = 50

  const nextImage = () => {
    setCurrentIndex((prev) => (prev === images.length - 1 ? 0 : prev + 1))
  }

  const prevImage = () => {
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1))
  }

  const handleThumbnailClick = (index: number) => {
    setCurrentIndex(index)
  }

  const handleZoomIn = () => {
    setModalOpen(true)
  }

  // Touch event handlers for swipe
  const onTouchStart = (e: React.TouchEvent) => {
    setTouchEnd(null)
    setTouchStart(e.targetTouches[0].clientX)
  }

  const onTouchMove = (e: React.TouchEvent) => {
    setTouchEnd(e.targetTouches[0].clientX)
  }

  const onTouchEnd = () => {
    if (!touchStart || !touchEnd) return

    const distance = touchStart - touchEnd
    const isLeftSwipe = distance > minSwipeDistance
    const isRightSwipe = distance < -minSwipeDistance

    if (isLeftSwipe) {
      nextImage()
    }
    if (isRightSwipe) {
      prevImage()
    }
  }

  return (
    <div className="relative">
      {/* Main image */}
      <div
        className="relative aspect-[16/9] md:aspect-[2/1] overflow-hidden rounded-lg"
        onTouchStart={onTouchStart}
        onTouchMove={onTouchMove}
        onTouchEnd={onTouchEnd}
      >
        <img
          src={images[currentIndex] || "/placeholder.svg"}
          alt={`${title} - Image ${currentIndex + 1}`}
          className="w-full h-full object-cover cursor-zoom-in"
          onClick={handleZoomIn}
        />

        <Button
          variant="secondary"
          size="icon"
          className="absolute left-4 top-1/2 -translate-y-1/2 rounded-full opacity-80 hover:opacity-100 z-10"
          onClick={prevImage}
        >
          <ChevronLeft className="h-6 w-6" />
        </Button>

        <Button
          variant="secondary"
          size="icon"
          className="absolute right-4 top-1/2 -translate-y-1/2 rounded-full opacity-80 hover:opacity-100 z-10"
          onClick={nextImage}
        >
          <ChevronRight className="h-6 w-6" />
        </Button>

        <div className="absolute bottom-4 right-4 bg-background/80 backdrop-blur-sm px-2 py-1 rounded-md text-sm">
          {currentIndex + 1} / {images.length}
        </div>

        <Button
          variant="secondary"
          size="icon"
          className="absolute top-4 right-4 rounded-full opacity-80 hover:opacity-100"
          onClick={handleZoomIn}
        >
          <ZoomIn className="h-5 w-5" />
        </Button>
      </div>

      {/* Thumbnails */}
      <div className="mt-4 grid grid-cols-5 gap-2">
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

      {/* Zoom Modal */}
      <ImageZoomModal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        images={images}
        initialIndex={currentIndex}
        title={title}
      />
    </div>
  )
}
