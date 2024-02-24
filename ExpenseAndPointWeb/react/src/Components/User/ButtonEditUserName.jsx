import { React, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import Cookies from 'universal-cookie';
import axios from 'axios';
import { EditUserNameUrl } from '../../Urls/UrlList';
import AlertModal from './../AlertModal.jsx';

const ButtonEditUserName = () => {
    const cookies = new Cookies();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };
    const cookiesLive = 604800; // Неделя
    const [showForm, setShowForm] = useState(false);
    const [userId, setUserId] = useState(cookies.get('userId'));
    const [user, setUser] = useState({
        id: cookies.get('userId'),
        name: ''
    });
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    const handleShowForm = () => {
        setShowForm(true);
    }

    const handleCloseForm = () => {
        setShowForm(false);
    }

    const ChangeName = (event) => {
        event.preventDefault();

        axios.put(EditUserNameUrl + userId, user, config)
            .then(res => {
                setShowAlertModal(true);
                setErrorMessage("Имя пользователя успешно изменено");
                setTitleAlert("Успешно");
                cookies.set('userName', user.name, { path: '/', maxAge: cookiesLive });
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Button variant="outline-light" size="lg" onClick={handleShowForm}>Сменить имя</Button>
            <Modal
                show={showForm}
                onHide={handleCloseForm}
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Изменение имени пользователя</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Label htmlFor="currentName">Текущее имя пользователя</Form.Label>
                    <Form.Control
                        id="currentName"
                        type="text"
                        placeholder={cookies.get('userName')}
                        readOnly
                    />
                    <Form.Label htmlFor="editedName">Новое имя пользователя</Form.Label>
                    <Form.Control
                        id="editedName"
                        type="text"
                        onChange={event => setUser({ ...user, name: event.target.value })} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="success" onClick={ChangeName }>Изменить</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ButtonEditUserName;
