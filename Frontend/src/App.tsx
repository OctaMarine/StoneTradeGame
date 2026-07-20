import { Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import LoginPage from './pages/LoginPage'
import RegistrationPage from './pages/RegistrationPage'
import GamePage from './pages/GamePage'
import LevelingPage from './pages/LevelingPage'

function App() {
   return (
       <Routes>
         <Route path="/" element={<Home />} />
         <Route path="/login" element={<LoginPage />} />
         <Route path="/registration" element={<RegistrationPage />} />
         <Route path="/game" element={<GamePage />} />
         <Route path="/leveling" element={<LevelingPage />} />
       </Routes>
     )
   }

 export default App