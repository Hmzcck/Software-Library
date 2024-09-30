import ItemCardContainer from "@/components/ItemCardContainer";

export default async function Home() {
  const response = await fetch("http://localhost:5079/api/items");
  const data = await response.json();
  console.log(JSON.stringify(data, null, 2)); 

  return (
    <div>
      <h1>Software Library</h1>
      <ItemCardContainer items={data} />
    </div>
  );
}
