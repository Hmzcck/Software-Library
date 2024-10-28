import { PaginationMetadata } from "@/types/Item";
import { useRouter, useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";

interface PaginationProps {
  metadata: PaginationMetadata;
  onPageChange?: (page: number) => void;
}

export default function Pagination({ metadata, onPageChange }: PaginationProps) {
  const [currentPage, setCurrentPage] = useState(metadata.pageNumber);
  const router = useRouter();
  const searchParams = useSearchParams();

  useEffect(() => {
    setCurrentPage(metadata.pageNumber);
  }, [metadata.pageNumber]);

  const handlePageChange = (page: number) => {
    if (page < 1 || page > metadata.totalPages) return;

    const params = new URLSearchParams(searchParams.toString());
    params.set("page", page.toString());
    router.push(`?${params.toString()}`);
    
    // Notify parent component if callback is provided
    onPageChange?.(page);
  };

  // Generate array of page numbers to display
  const getPageNumbers = () => {
    const delta = 2; // Number of pages to show before and after current page
    const range = [];
    const rangeWithDots = [];

    for (
      let i = Math.max(2, currentPage - delta);
      i <= Math.min(metadata.totalPages - 1, currentPage + delta);
      i++
    ) {
      range.push(i);
    }

    if (currentPage - delta > 2) {
      rangeWithDots.push(1, "...");
    } else {
      rangeWithDots.push(1);
    }

    rangeWithDots.push(...range);

    if (currentPage + delta < metadata.totalPages - 1) {
      rangeWithDots.push("...", metadata.totalPages);
    } else if (metadata.totalPages > 1) {
      rangeWithDots.push(metadata.totalPages);
    }

    return rangeWithDots;
  };

  // Don't render pagination if there's only one page
  if (metadata.totalPages <= 1) return null;

  return (
    <nav aria-label="Page navigation" className="my-4">
      <div className="flex flex-col items-center gap-2">
        <ul className="list-style-none flex items-center justify-center gap-1">
          <li>
            <button
              onClick={() => handlePageChange(currentPage - 1)}
              disabled={!metadata.hasPrevious}
              className="relative block rounded bg-transparent px-3 py-1.5 text-sm text-surface transition duration-300 hover:bg-neutral-100 focus:bg-neutral-100 focus:text-primary-700 focus:outline-none focus:ring-0 active:bg-neutral-100 active:text-primary-700 dark:text-white dark:hover:bg-neutral-700 dark:focus:bg-neutral-700 disabled:opacity-50 disabled:cursor-not-allowed"
              aria-label="Previous page"
            >
              Previous
            </button>
          </li>

          {getPageNumbers().map((page, index) => (
            <li key={index}>
              {page === "..." ? (
                <span className="px-3 py-1.5 text-sm text-surface dark:text-white">
                  ...
                </span>
              ) : (
                <button
                  onClick={() => handlePageChange(page as number)}
                  className={`relative block rounded px-3 py-1.5 text-sm transition duration-300 hover:bg-neutral-100 focus:outline-none dark:text-white dark:hover:bg-neutral-700 ${
                    currentPage === page
                      ? "bg-primary-600 text-white hover:bg-primary-700 dark:bg-primary-500 dark:hover:bg-primary-600"
                      : "bg-transparent text-surface"
                  }`}
                  aria-label={`Page ${page}`}
                  aria-current={currentPage === page ? "page" : undefined}
                >
                  {page}
                </button>
              )}
            </li>
          ))}

          <li>
            <button
              onClick={() => handlePageChange(currentPage + 1)}
              disabled={!metadata.hasNext}
              className="relative block rounded bg-transparent px-3 py-1.5 text-sm text-surface transition duration-300 hover:bg-neutral-100 focus:bg-neutral-100 focus:text-primary-700 focus:outline-none focus:ring-0 active:bg-neutral-100 active:text-primary-700 dark:text-white dark:hover:bg-neutral-700 dark:focus:bg-neutral-700 disabled:opacity-50 disabled:cursor-not-allowed"
              aria-label="Next page"
            >
              Next
            </button>
          </li>
        </ul>
        <div className="text-sm text-gray-600 dark:text-gray-400">
          Showing page {currentPage} of {metadata.totalPages} ({metadata.totalCount} items)
        </div>
      </div>
    </nav>
  );
}