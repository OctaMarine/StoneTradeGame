import { Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Login from './pages/Login'
import Registration from './pages/Registration'
import Game from './pages/Game'

function App() {
   return (
       <Routes>
         <Route path="/" element={<Home />} />
         <Route path="/login" element={<Login />} />
         <Route path="/registration" element={<Registration />} />
         <Route path="/game" element={<Game />} />
       </Routes>
     )
   }

 export default App