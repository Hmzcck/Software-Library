export type ItemCardProps = {
    id: number;
    name: string;
    categoryNames: Array<string>;
    publisher: string;
    description: string;
    stars: number;
    forks: number;
    repository: string;
    image: string;
    creationDate: string;
  };

  export type ItemCardContainerProps = {
    items: Array<ItemCardProps>; // Reference to ItemCardProps instead of redefining
  };

  export type ItemDetail = {
    id: number;
    name: string;
    description: string;
    publisher: string;
    categoryNames: Array<string>;
    stars: number;
    forks: number;
    repository: string;
    image: string;
    creationDate: string;
    reviews: Array<{
      id: number;
      name: string;
      rating: number;
      comment: string;
      createdAt: string;
      createdBy: string;
    }>;
  };