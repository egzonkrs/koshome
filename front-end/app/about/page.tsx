"use client"

import { Header } from "@/components/header"
import { Card, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Home, Users, Award, MapPin, Heart, Shield, Clock, Star } from "lucide-react"

const stats = [
  { label: "Properties Listed", value: "500+", icon: Home },
  { label: "Happy Clients", value: "1,200+", icon: Users },
  { label: "Years Experience", value: "10+", icon: Award },
  { label: "Cities Covered", value: "8", icon: MapPin },
]

const values = [
  {
    icon: Heart,
    title: "Customer First",
    description: "We prioritize your needs and work tirelessly to find your perfect home.",
  },
  {
    icon: Shield,
    title: "Trust & Transparency",
    description: "Honest dealings and clear communication are at the core of everything we do.",
  },
  {
    icon: Clock,
    title: "Always Available",
    description: "Our dedicated team is available to assist you throughout your property journey.",
  },
  {
    icon: Star,
    title: "Quality Service",
    description: "We maintain the highest standards in property listings and customer service.",
  },
]

const team = [
  {
    name: "Filan Fisteku",
    role: "Founder & CEO",
    image: "/professional-man-portrait.png",
    description: "With over 15 years in real estate, Filan founded KosHome to modernize property search in Kosovo.",
  },
  {
    name: "Filan Fistekuaj",
    role: "Head of Sales",
    image: "/professional-woman-portrait.png",
    description: "Filan leads our sales team with expertise in luxury properties and client relations.",
  },
  {
    name: "Filan Fistekuqaj",
    role: "Property Specialist",
    image: "/professional-man-portrait.png",
    description: "Filan's deep knowledge of Prishtina neighborhoods helps clients find their ideal location.",
  },
]

export default function AboutPage() {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />

      <main className="flex-1">
        {/* Hero Section */}
        <section className="container mx-auto px-4 py-16 text-center">
          <Badge variant="secondary" className="mb-4">
            About Us
          </Badge>
          <h1 className="text-4xl md:text-5xl font-bold mb-6">
            Your Trusted Partner in
            <br />
            Finding the Perfect Home
          </h1>
          <p className="text-lg text-muted-foreground max-w-3xl mx-auto mb-8">
            KosHome is Kosovo's leading real estate platform, dedicated to helping individuals and families find their
            dream homes. We combine local expertise with modern technology to make your property search simple and
            enjoyable.
          </p>
        </section>

        {/* Stats Section */}
        <section className="bg-primary/5 py-16">
          <div className="container mx-auto px-4">
            <div className="grid grid-cols-2 md:grid-cols-4 gap-8">
              {stats.map((stat) => (
                <div key={stat.label} className="text-center">
                  <stat.icon className="h-10 w-10 text-primary mx-auto mb-4" />
                  <div className="text-3xl md:text-4xl font-bold mb-2">{stat.value}</div>
                  <div className="text-muted-foreground">{stat.label}</div>
                </div>
              ))}
            </div>
          </div>
        </section>

        {/* Mission Section */}
        <section className="container mx-auto px-4 py-16">
          <div className="max-w-4xl mx-auto text-center">
            <h2 className="text-3xl font-bold mb-6">Our Mission</h2>
            <p className="text-lg text-muted-foreground">
              We believe that finding a home should be an exciting journey, not a stressful ordeal. Our mission is to
              revolutionize the real estate experience in Kosovo by providing a transparent, efficient, and personalized
              service that puts your needs first. Whether you're buying your first apartment, renting in a new city, or
              investing in property, we're here to guide you every step of the way.
            </p>
          </div>
        </section>

        {/* Values Section */}
        <section className="bg-primary/5 py-16">
          <div className="container mx-auto px-4">
            <h2 className="text-3xl font-bold text-center mb-12">Our Values</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
              {values.map((value) => (
                <Card key={value.title} className="text-center">
                  <CardContent className="pt-6">
                    <value.icon className="h-12 w-12 text-primary mx-auto mb-4" />
                    <h3 className="text-xl font-semibold mb-2">{value.title}</h3>
                    <p className="text-muted-foreground">{value.description}</p>
                  </CardContent>
                </Card>
              ))}
            </div>
          </div>
        </section>

        {/* Team Section */}
        <section className="container mx-auto px-4 py-16">
          <h2 className="text-3xl font-bold text-center mb-4">Meet Our Team</h2>
          <p className="text-muted-foreground text-center mb-12 max-w-2xl mx-auto">
            Our experienced team of real estate professionals is dedicated to helping you achieve your property goals.
          </p>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
            {team.map((member) => (
              <Card key={member.name} className="overflow-hidden">
                <div className="aspect-square relative">
                  <img
                    src={member.image || "/placeholder.svg"}
                    alt={member.name}
                    className="w-full h-full object-cover"
                  />
                </div>
                <CardContent className="p-6 text-center">
                  <h3 className="text-xl font-semibold mb-1">{member.name}</h3>
                  <Badge variant="secondary" className="mb-3">
                    {member.role}
                  </Badge>
                  <p className="text-sm text-muted-foreground">{member.description}</p>
                </CardContent>
              </Card>
            ))}
          </div>
        </section>

        {/* CTA Section */}
        <section className="bg-primary text-primary-foreground py-16">
          <div className="container mx-auto px-4 text-center">
            <h2 className="text-3xl font-bold mb-4">Ready to Find Your Home?</h2>
            <p className="text-lg opacity-90 mb-8 max-w-2xl mx-auto">
              Browse our extensive collection of properties or get in touch with our team for personalized assistance.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <a
                href="/apartments"
                className="inline-flex items-center justify-center rounded-md text-sm font-medium bg-background text-foreground h-11 px-8 hover:bg-background/90 transition-colors"
              >
                Browse Properties
              </a>
              <a
                href="/contact"
                className="inline-flex items-center justify-center rounded-md text-sm font-medium border border-primary-foreground/20 h-11 px-8 hover:bg-primary-foreground/10 transition-colors"
              >
                Contact Us
              </a>
            </div>
          </div>
        </section>
      </main>
    </div>
  )
}
