type PageHeaderProps = {
  title: string;
  description: string;
};

function PageHeader({ title, description }: PageHeaderProps) {
  return (
    <header>
      <h2>{title}</h2>
      <p>{description}</p>
    </header>
  );
}

export default PageHeader;
