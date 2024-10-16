export type ItemCardProps = {
    id: number;
    name: string;
    categoryNames: Array<string>;
    publisher: string;
    description: string;
    image: string;
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
    image: string;
    isPaid: boolean;
    reviews: Array<{
      id: number;
      name: string;
      rating: number;
      comment: string;
      createdAt: string;
      createdBy: string;
    }>;
  };