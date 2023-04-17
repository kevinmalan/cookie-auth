import Head from 'next/head'
import { useState } from 'react'

export default function Home() {
  const [state, setState] = useState({
    username: '',
    password: ''
  })

  const handleUsernameChange = (event: { target: { value: string } }) => {
    setState({
      ...state,
      username: event.target.value
    })
  }

  const handlePasswordChange = (event: { target: { value: string } }) => {
    setState({
      ...state,
      password: event.target.value
    })
  }

  let handleSubmit = (event: any) => {
    event.preventDefault();
    // TODO: Send to API
  }

  return (
    <>
      <Head>
        <title>Login</title>
        <meta name="description" content="Login to your profile" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>
      <main>
        <h1>Sign In</h1>
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="username">Username</label>
            <input type="text" id="username" value={state.username} onChange={handleUsernameChange} />
          </div>
          <div>
            <label htmlFor="password">Password</label>
            <input type="password" id="password" value={state.password} onChange={handlePasswordChange} />
          </div>
          <button type="submit">Next</button>
          <div className="register">
            <p>Don't have a profile? <a href="#" style={{ color: "#4CAF50" }}>Register</a></p>
          </div>
        </form>
      </main>
    </>
  )
}
