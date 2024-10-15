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