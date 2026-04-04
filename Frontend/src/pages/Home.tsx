import { Link } from 'react-router-dom';

export default function Home() {
  return (
    <main>
      <div className="max-w-4xl mx-auto flex items-center justify-center min-h-screen">
        <Link to="/login">
          <button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors">
            Play
          </button>
        </Link>
      </div>
    </main>
  );
}