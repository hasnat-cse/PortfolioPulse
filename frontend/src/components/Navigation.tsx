import "./Navigation.css";

type NavigationProps = {
  activeItem: string;
};

const navigationItems = [
  { label: "Dashboard", href: "#" },
  { label: "Accounts", href: "#accounts" },
  { label: "Holdings", href: "#holdings" },
];

function Navigation({ activeItem }: NavigationProps) {
  return (
    <nav>
      {navigationItems.map((item) => (
        <a
          key={item.href}
          href={item.href}
          className={item.label === activeItem ? "active" : ""}
        >
          {item.label}
        </a>
      ))}
    </nav>
  );
}

export default Navigation;
