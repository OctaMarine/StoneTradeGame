import Game from '@/components/features/Game/Game';

export default function GamePage() {
    // Здесь можно добавить:
    // - Проверку авторизации
    // - Получение параметров из URL
    // - Обертки (Layout, провайдеры)
    
    // Пример с проверкой авторизации:
    // const { isAuthenticated } = useAuth();
    // if (!isAuthenticated) return <Navigate to="/login" />;
    
    // Пример с параметрами URL:
    // const { gameId } = useParams();
    // return <Game gameId={gameId} />;
    
    return <Game />;
}