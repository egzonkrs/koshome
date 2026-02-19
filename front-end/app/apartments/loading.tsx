import { Skeleton } from "@/components/ui/skeleton"
import { Header } from "@/components/header"

export default function Loading() {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />

      <main className="flex-1 container mx-auto px-4 py-8">
        <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
          <div>
            <Skeleton className="h-10 w-48" />
            <Skeleton className="h-5 w-32 mt-2" />
          </div>

          <div className="flex items-center gap-4">
            <Skeleton className="h-10 w-32" />
            <Skeleton className="h-10 w-24" />
          </div>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-[300px_1fr] gap-8">
          <aside className="bg-[#eeefe9] p-6 rounded-lg shadow-sm h-fit">
            <Skeleton className="h-8 w-32 mb-4" />
            <Skeleton className="h-10 w-full mb-6" />

            <div className="space-y-6">
              <div>
                <Skeleton className="h-5 w-24 mb-2" />
                <Skeleton className="h-10 w-full" />
              </div>

              <div>
                <Skeleton className="h-5 w-24 mb-2" />
                <Skeleton className="h-10 w-full" />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <Skeleton className="h-5 w-20 mb-2" />
                  <Skeleton className="h-10 w-full" />
                </div>
                <div>
                  <Skeleton className="h-5 w-20 mb-2" />
                  <Skeleton className="h-10 w-full" />
                </div>
              </div>
            </div>
          </aside>

          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {Array.from({ length: 6 }).map((_, i) => (
                <div key={i} className="bg-[#eeefe9] rounded-lg overflow-hidden shadow-sm">
                  <Skeleton className="h-48 w-full" />
                  <div className="p-4">
                    <Skeleton className="h-6 w-3/4 mb-2" />
                    <Skeleton className="h-4 w-1/2 mb-4" />
                    <div className="flex justify-between">
                      <Skeleton className="h-5 w-20" />
                      <Skeleton className="h-5 w-16" />
                    </div>
                  </div>
                </div>
              ))}
            </div>

            <div className="flex justify-center">
              <Skeleton className="h-10 w-64" />
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
