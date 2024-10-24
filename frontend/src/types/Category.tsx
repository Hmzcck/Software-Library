import { ItemCardContainerProps } from "./Item";

export type Category = {
  id: number;
  name: string;
  description: string;
  items: ItemCardContainerProps;
};
