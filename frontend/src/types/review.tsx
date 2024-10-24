export type Review = {
  id: number;
  name: string;
  rating: number;
  comment: string;
  createdAt: string;
  createdBy: string;
};

export type ReviewListProps = {
  reviews: Review[] | null;
  itemId: number;
  isLoggedIn: boolean;
  token: string;
};

export type ReviewData = {
  itemId: number;
  name: string;
  rating: number;
  comment: string;
};
