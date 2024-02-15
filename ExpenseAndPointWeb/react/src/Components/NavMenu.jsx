import { React, useState } from 'react';
import { Navbar, Container, Nav, Button } from 'react-bootstrap'; 
import Cookies from 'universal-cookie';

const NavMenu = () => {
    const cookies = new Cookies();

    const SignOut = (event) => {
        event.preventDefault();

        cookies.set('access_token', "", { path: '/' });
        cookies.set('userId', "", { path: '/' });
        cookies.set('userName', "", { path: '/' });
        window.location.replace('http://localhost:5173/auth')
    }

    return (
        <>
            <Navbar fixed="top" collapseOnSelect expand="md" bg="dark" variant="dark" >
                <Container>
                    <Navbar.Toggle aria-controls="responsive-navbar-nav" />
                    <Navbar.Collapse id="responsive-navbar-nar">
                        <Nav className="me-auto">
                            <Nav.Link href="/">День</Nav.Link>
                            <Nav.Link href="/month">Месяц</Nav.Link>
                            <Nav.Link href="/period">Период</Nav.Link>
                        </Nav>
                        <Button variant="dark" className="justify-content-end" disabled>{cookies.get('userName') }</Button>
                        <Button variant="outline-light" className="justify-content-end"
                            onClick={SignOut}
                        >Выход</Button>
                        
                    </Navbar.Collapse>
                </Container>
            </Navbar>

        </>
    );
}

export default NavMenu;