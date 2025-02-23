import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
//import './styles.css'; // Ensure this path is correct
import { Routes, Route, Navigate } from 'react-router-dom';
import { Provider } from 'react-redux';
import store from './store';
import { useLocation } from 'react-router-dom';
// Component Imports
import Header from './Header';
import Login from './Login';
import ContactUs from './ContactUs';
import FAQ from './FAQ';
import Amenities from './Amenities';
import AdminDashboard from './AdminDashboard';
import LandingPage from './LandingPage';
import RegistrationForm from './Registration';
import PrivateRoute from './PrivateRoute';
import PublicRoute from './PublicRoute';
import Profile from './Profile';
import RoomAllocation from './RoomAllocation';
import FoodMealManager from './AdminMeal';
import MealComponent from './MealComponent';
import TicketList from './TicketList';
import TicketForm from './TicketForm';
import AdminTicketSystem from './AdminTicketSystem';
import AllocatedHostlers from './AllocatedHostler';
import ForgetPassword from './ForgetPassword';
import OwnerDashboard from './OwnerDashboard';
import RoomStatusBoard from './RoomStatusBoard';

import '../src/App.css';
import HostellerDashboard from './HostlerDashboard';



const App = () => {
    return (
        <Provider store={store}>
            <div>
                <Header />
                <div className="container">
                    <Routes>
                        {/* Redirect from root to admin */}
                       
                        
                        {/* Public Routes */}
                        <Route path='/' element={<LandingPage />} />
                        <Route path="/login" element={<PublicRoute><Login /></PublicRoute>} />
                        <Route path='/register' element={<RegistrationForm/>}/>
                        <Route path="/contact" element={<ContactUs />} />
                        <Route path="/faq" element={<FAQ />} />
                        <Route path="/amenities" element={<Amenities />} />
                        <Route path='/forgetPassword' element={<ForgetPassword></ForgetPassword>}/>
                       
                       
                       <Route path="/roomStatus" element={<PrivateRoute><RoomStatusBoard></RoomStatusBoard></PrivateRoute>}></Route>
                        {/* Protected Routes */}
                        <Route path="/owner" element={<PrivateRoute><OwnerDashboard></OwnerDashboard></PrivateRoute>}></Route>
                       <Route path="/roomAllocate" element={<PrivateRoute><AllocatedHostlers></AllocatedHostlers></PrivateRoute>}/> 
                        <Route path="/admin" element={<PrivateRoute><AdminDashboard /></PrivateRoute>} />
                        <Route path="/meal" element={<PrivateRoute><FoodMealManager></FoodMealManager></PrivateRoute>}/>
                        <Route path="/hostler-dashboard" element={<PrivateRoute><HostellerDashboard /></PrivateRoute>} />
                        <Route path="/hostelerprofile" element={<PrivateRoute><Profile/></PrivateRoute>} />
                        <Route path="/roomallocate" element={<PrivateRoute><RoomAllocation/></PrivateRoute>} />
                        {/* Fallback Route */}
                        <Route path="/hostlerMeal" element={<PrivateRoute><MealComponent></MealComponent></PrivateRoute>}/>
                        <Route path="/ticketlist" element= {<PrivateRoute><TicketList/></PrivateRoute>} />
                        <Route path="/ticketform" element={<PrivateRoute><TicketForm/></PrivateRoute>} />
                        <Route path="/ticket" element={<PrivateRoute><AdminTicketSystem/></PrivateRoute>}/>
                        
                        
                        <Route path="*" element={<h2>404 Not Found</h2>} />
                        
                        </Routes>
                        
                </div>
            </div>
        </Provider>
    );
};



export default App;
