import { parseISO, format } from 'date-fns';


export const formatDate = (creationDate: string) => {
    try {
      const date = parseISO(creationDate); 
      return format(date, "MMM d, yyyy"); 
    } catch (error) {
      console.error("Error parsing date:", error);
      return creationDate; 
    }
  };