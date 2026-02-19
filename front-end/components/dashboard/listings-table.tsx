"use client"

import { useState } from "react"
import Image from "next/image"
import { Edit, MoreHorizontal, Trash2, Eye, MessageSquare } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog"
import { useLandlordStore, type LandlordListing, type ListingStatus } from "@/store/landlord-store"
import { useToast } from "@/hooks/use-toast"

interface ListingsTableProps {
  listings: LandlordListing[]
  onEdit: (listing: LandlordListing) => void
}

const statusColors: Record<ListingStatus, string> = {
  active: "bg-green-500/10 text-green-600 border-green-200",
  pending: "bg-yellow-500/10 text-yellow-600 border-yellow-200",
  inactive: "bg-gray-500/10 text-gray-600 border-gray-200",
  sold: "bg-blue-500/10 text-blue-600 border-blue-200",
  rented: "bg-purple-500/10 text-purple-600 border-purple-200",
}

export function ListingsTable({ listings, onEdit }: ListingsTableProps) {
  const { toast } = useToast()
  const { deleteListing } = useLandlordStore()
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false)
  const [listingToDelete, setListingToDelete] = useState<LandlordListing | null>(null)

  const handleDelete = () => {
    if (listingToDelete) {
      deleteListing(listingToDelete.id)
      toast({
        title: "Listing Deleted",
        description: "The listing has been permanently removed.",
      })
      setDeleteDialogOpen(false)
      setListingToDelete(null)
    }
  }

  const confirmDelete = (listing: LandlordListing) => {
    setListingToDelete(listing)
    setDeleteDialogOpen(true)
  }

  const formatPrice = (price: number, forRent: boolean) => {
    if (forRent) {
      return `€${price.toLocaleString()}/mo`
    }
    return `€${price.toLocaleString()}`
  }

  if (listings.length === 0) {
    return (
      <div className="text-center py-12 bg-muted/30 rounded-lg border border-dashed">
        <p className="text-muted-foreground">No listings yet. Create your first listing to get started!</p>
      </div>
    )
  }

  return (
    <>
      <div className="rounded-lg border bg-card overflow-hidden">
        <Table>
          <TableHeader>
            <TableRow className="bg-muted/50">
              <TableHead className="w-[80px]">Image</TableHead>
              <TableHead>Title</TableHead>
              <TableHead>Price</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-center">
                <Eye className="h-4 w-4 inline-block mr-1" />
                Views
              </TableHead>
              <TableHead className="text-center">
                <MessageSquare className="h-4 w-4 inline-block mr-1" />
                Inquiries
              </TableHead>
              <TableHead className="w-[80px]">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {listings.map((listing) => {
              const location = listing.location ?? { address: "", district: "" }
              const images = listing.images ?? []
              const listingType = listing.listingType?.toString().toLowerCase()
              const isForRent = listing.forRent ?? listingType === "rent"
              const isForSale = listing.forSale ?? listingType === "sale"
              const title = listing.title || "Listing"

              return (
                <TableRow key={listing.id} className="hover:bg-muted/30">
                  <TableCell>
                    <div className="relative h-12 w-12 rounded-md overflow-hidden">
                      <Image
                        src={images[0] || "/placeholder.svg"}
                        alt={title}
                        fill
                        className="object-cover"
                      />
                    </div>
                  </TableCell>
                  <TableCell>
                    <div>
                      <p className="font-medium line-clamp-1">{title}</p>
                      <p className="text-sm text-muted-foreground line-clamp-1">
                        {location.address}, {location.district}
                      </p>
                    </div>
                  </TableCell>
                  <TableCell>
                    <span className="font-semibold">{formatPrice(listing.price, isForRent && !isForSale)}</span>
                  </TableCell>
                <TableCell>
                  <Badge variant="outline" className={statusColors[listing.status]}>
                    {listing.status.charAt(0).toUpperCase() + listing.status.slice(1)}
                  </Badge>
                </TableCell>
                <TableCell className="text-center">
                  <span className="text-muted-foreground">{listing.views}</span>
                </TableCell>
                <TableCell className="text-center">
                  <span className="text-muted-foreground">{listing.inquiries}</span>
                </TableCell>
                <TableCell>
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" size="icon">
                        <MoreHorizontal className="h-4 w-4" />
                        <span className="sr-only">Actions</span>
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuItem onClick={() => onEdit(listing)}>
                        <Edit className="h-4 w-4 mr-2" />
                        Edit
                      </DropdownMenuItem>
                      <DropdownMenuSeparator />
                      <DropdownMenuItem
                        onClick={() => confirmDelete(listing)}
                        className="text-destructive focus:text-destructive"
                      >
                        <Trash2 className="h-4 w-4 mr-2" />
                        Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
                </TableRow>
              )
            })}
          </TableBody>
        </Table>
      </div>

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Listing</AlertDialogTitle>
            <AlertDialogDescription>
              Are you sure you want to delete "{listingToDelete?.title}"? This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={handleDelete}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  )
}
