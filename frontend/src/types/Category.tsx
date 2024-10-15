import { ItemCardContainerProps } from "./Item";


export type Category = {
    id: number; // Adjust the type according to your API response
    name: string;
    description: string;
    items: ItemCardContainerProps; // Use an appropriate type based on your data
  }