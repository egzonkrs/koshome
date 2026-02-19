import type { ApartmentLocation, ApartmentResponse } from "./api/types"

interface LegacyApartment extends Omit<ApartmentResponse, "squareMeters" | "address" | "listingType"> {
  location: ApartmentLocation
  area: number
  pricePerSqm: number
  floor: number
  features: string[]
  images: string[]
  forSale: boolean
  forRent: boolean
}

const legacyApartments: LegacyApartment[] = [
  {
    id: "apt-001",
    title: "Modern Apartment in City Center",
    description:
      "Beautiful modern apartment located in the heart of the city. Features a spacious living room, fully equipped kitchen, and a balcony with city views.",
    price: 120000,
    pricePerSqm: 1500,
    area: 80,
    bedrooms: 2,
    bathrooms: 1,
    floor: 3,
    location: {
      address: "123 Main Street",
      city: "Pristina",
      district: "City Center",
      country: "Kosovo",
      coordinates: [42.6629, 21.1655],
    },
    features: ["Elevator", "Parking", "Security", "Air Conditioning"],
    images: [
      "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2023-01-15T10:30:00Z",
    updatedAt: "2023-01-15T10:30:00Z",
  },
  {
    id: "apt-002",
    title: "Luxury Penthouse with Terrace",
    description:
      "Exclusive penthouse with a large terrace offering panoramic views of the city. Features high-end finishes, smart home system, and private elevator access.",
    price: 350000,
    pricePerSqm: 2800,
    area: 125,
    bedrooms: 3,
    bathrooms: 2,
    floor: 10,
    location: {
      address: "45 Park Avenue",
      city: "Pristina",
      district: "Arbëri",
      country: "Kosovo",
      coordinates: [42.6729, 21.1755],
    },
    features: ["Terrace", "Smart Home", "Private Elevator", "Luxury Finishes", "Security System"],
    images: [
      "https://images.unsplash.com/photo-1567767292278-a4f21aa2d36e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2023-02-20T14:45:00Z",
    updatedAt: "2023-02-20T14:45:00Z",
  },
  {
    id: "apt-003",
    title: "Cozy Studio Apartment",
    description:
      "Charming studio apartment perfect for students or young professionals. Recently renovated with modern furnishings and appliances.",
    price: 65000,
    pricePerSqm: 1300,
    area: 50,
    bedrooms: 0,
    bathrooms: 1,
    floor: 2,
    location: {
      address: "78 University Street",
      city: "Pristina",
      district: "Dardania",
      country: "Kosovo",
      coordinates: [42.6529, 21.1555],
    },
    features: ["Furnished", "Internet", "Washing Machine", "Refrigerator"],
    images: [
      "https://images.unsplash.com/photo-1554995207-c18c203602cb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1630699144867-37acec97df5a?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1560185007-cde436f6a4d0?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: true,
    createdAt: "2023-03-05T09:15:00Z",
    updatedAt: "2023-03-05T09:15:00Z",
  },
  {
    id: "apt-004",
    title: "Family Apartment with Garden",
    description:
      "Spacious family apartment with a private garden. Located in a quiet residential area with good schools and parks nearby.",
    price: 180000,
    pricePerSqm: 1500,
    area: 120,
    bedrooms: 3,
    bathrooms: 2,
    floor: 0,
    location: {
      address: "15 Green Street",
      city: "Pristina",
      district: "Kalabria",
      country: "Kosovo",
      coordinates: [42.6429, 21.1455],
    },
    features: ["Garden", "Parking", "Storage", "Playground", "Pet Friendly"],
    images: [
      "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600585154340-be6161a56a0c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600566753190-17f0baa2a6c3?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2023-04-10T11:20:00Z",
    updatedAt: "2023-04-10T11:20:00Z",
  },
  {
    id: "apt-005",
    title: "Modern Loft in Historic Building",
    description:
      "Unique loft apartment in a renovated historic building. Features high ceilings, exposed brick walls, and modern amenities.",
    price: 145000,
    pricePerSqm: 1800,
    area: 80,
    bedrooms: 1,
    bathrooms: 1,
    floor: 4,
    location: {
      address: "32 Old Town Road",
      city: "Pristina",
      district: "Old Town",
      country: "Kosovo",
      coordinates: [42.6629, 21.1755],
    },
    features: ["High Ceilings", "Exposed Brick", "Open Floor Plan", "Hardwood Floors"],
    images: [
      "https://images.unsplash.com/photo-1560448075-bb485b067938?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600210492493-0946911123ea?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2023-05-15T13:40:00Z",
    updatedAt: "2023-05-15T13:40:00Z",
  },
  {
    id: "apt-006",
    title: "Renovated Apartment with Balcony",
    description:
      "Freshly renovated apartment with a balcony overlooking the park. New kitchen, bathroom, and flooring throughout.",
    price: 95000,
    pricePerSqm: 1350,
    area: 70,
    bedrooms: 2,
    bathrooms: 1,
    floor: 5,
    location: {
      address: "56 Park View",
      city: "Pristina",
      district: "Ulpiana",
      country: "Kosovo",
      coordinates: [42.6529, 21.1655],
    },
    features: ["Balcony", "New Kitchen", "New Bathroom", "Park View"],
    images: [
      "https://images.unsplash.com/photo-1493809842364-78817add7ffb?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600585154526-990dced4db0d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600573472550-8090b5e0745e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: true,
    createdAt: "2023-06-20T15:10:00Z",
    updatedAt: "2023-06-20T15:10:00Z",
  },
  {
    id: "apt-007",
    title: "Luxury Apartment with Mountain View",
    description:
      "High-end apartment with stunning mountain views. Features premium finishes, spacious rooms, and a modern kitchen.",
    price: 220000,
    pricePerSqm: 2000,
    area: 110,
    bedrooms: 3,
    bathrooms: 2,
    floor: 7,
    location: {
      address: "89 Mountain Road",
      city: "Pristina",
      district: "Veternik",
      country: "Kosovo",
      coordinates: [42.6729, 21.1455],
    },
    features: ["Mountain View", "Premium Finishes", "Spacious", "Modern Kitchen", "Garage"],
    images: [
      "https://images.unsplash.com/photo-1600047509807-ba8f99d2cdde?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600566753376-12c8ab7fb75b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600210491892-03d54c0aaf87?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: false,
    createdAt: "2023-07-25T16:30:00Z",
    updatedAt: "2023-07-25T16:30:00Z",
  },
  {
    id: "apt-008",
    title: "Affordable Studio Near University",
    description:
      "Compact and affordable studio apartment located near the university. Ideal for students or as an investment property.",
    price: 45000,
    pricePerSqm: 1125,
    area: 40,
    bedrooms: 0,
    bathrooms: 1,
    floor: 1,
    location: {
      address: "12 College Street",
      city: "Pristina",
      district: "Bregu i Diellit",
      country: "Kosovo",
      coordinates: [42.6429, 21.1555],
    },
    features: ["Near University", "Public Transport", "Affordable", "Investment Opportunity"],
    images: [
      "https://images.unsplash.com/photo-1560448204-61dc36dc98c8?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1600121848594-d8644e57abab?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
      "https://images.unsplash.com/photo-1598928506311-c55ded91a20c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=2340&q=80",
    ],
    forSale: true,
    forRent: true,
    createdAt: "2023-08-30T08:50:00Z",
    updatedAt: "2023-08-30T08:50:00Z",
  },
]

export const apartments: ApartmentResponse[] = legacyApartments.map((apartment) => ({
  ...apartment,
  squareMeters: apartment.area,
  address: apartment.location.address,
  listingType: apartment.forSale ? "sale" : "rent",
}))
