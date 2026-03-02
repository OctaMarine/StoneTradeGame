'use client';

import { useState, FormEvent } from 'react';
import { api } from '@/lib/api';
import Link from 'next/link';

export default function RegistrationPage() {
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (event: FormEvent) => {
        event.preventDefault();
        setError(null);
        setLoading(true);

        try {
            const responseData = await api.auth.register(userName, password, email);

            console.log('Registration successful');
            console.log('Server response:', responseData);

            // После успешной регистрации перенаправляем на страницу входа
            window.location.href = '/login';

        } catch (error) {
            console.error('An error occurred during registration:', error);
            setError(error instanceof Error ? error.message : 'An unknown error occurred.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', width: '300px', gap: '10px' }}>
                <h1>Registration</h1>
                <input
                    type="text"
                    placeholder="Username"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    style={{ padding: '8px' }}
                    disabled={loading}
                    required
                />
                <input
                    type="text"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    style={{ padding: '8px' }}
                    disabled={loading}
                    required
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    style={{ padding: '8px' }}
                    disabled={loading}
                    required
                />
                <button type="submit" style={{ padding: '10px' }} disabled={loading}>
                    {loading ? 'Registering...' : 'Register'}
                </button>
                {error && <p style={{ color: 'red' }}>{error}</p>}
                <div style={{ textAlign: 'center', marginTop: '10px' }}>
                    <Link href="/login" style={{ color: 'white', textDecoration: 'underline' }}>
                        Back to Login
                    </Link>
                </div>
            </form>
        </div>
    );
}