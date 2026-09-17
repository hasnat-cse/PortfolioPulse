import "./AppHeader.css";
import Navigation from "./Navigation";

type AppHeaderProps = {
  title: string;
};

function AppHeader({ title }: AppHeaderProps) {
  return (
    <header>
      <h1>{title}</h1>
      <Navigation activeItem="Dashboard" />
    </header>
  );
}

export default AppHeader;
