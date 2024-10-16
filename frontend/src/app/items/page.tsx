import ItemCardContainer from "@/components/home/ItemCardContainer";
import { fetchItems } from "@/services/item";  // Import the service

export default async function items() {
  const data = await fetchItems();  // Use the service function
 // console.log(JSON.stringify(data, null, 2));

  return (
    <div>
      <h1>Software Library</h1>
      <ItemCardContainer items={data} />
    </div>
  );
}
