import { React, useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import Cookies from 'universal-cookie';
import axios from 'axios';
import { EditUserPasswordUrl } from '../../Urls/UrlList';
import AlertModal from './../AlertModal.jsx';

const ButtonEditUserPassword = () => {
    const cookies = new Cookies();
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };
    const [showForm, setShowForm] = useState(false);
    const [userId, setUserId] = useState(cookies.get('userId'));
    const [user, setUser] = useState({
        id: cookies.get('userId'),
        name: '',
        password: '',
        oldPassword: ''
    });
    const [confirmedPassword, setConfirmedPassword] = useState();
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    const handleShowForm = () => {
        setShowForm(true);
    }

    const handleCloseForm = () => {
        setShowForm(false);
    }

    const ChangePassword = (event) => {
        event.preventDefault();
        if (user.password !== confirmedPassword) {
            setShowAlertModal(true);
            setErrorMessage("Новый пароль и подтвержденный пароль должны совпадать!");
            setTitleAlert("Ошибка");
            return;
        }

        axios.put(EditUserPasswordUrl + userId, user, config)
            .then(res => {
                setShowAlertModal(true);
                setErrorMessage("Пароль успешно изменен");
                setTitleAlert("Успешно");
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
            <Button variant="outline-light" size="lg" onClick={handleShowForm}>Сменить пароль</Button>
            <Modal
                show={showForm}
                onHide={handleCloseForm}
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Изменение пароля</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Label htmlFor="oldPassword">Старый пароль</Form.Label>
                    <Form.Control
                        id="oldPassword"
                        type="password"
                        onChange={event => setUser({ ...user, oldPassword: event.target.value })} />
                    <Form.Label htmlFor="newPassword">Новый пароль</Form.Label>
                    <Form.Control
                        id="newPassword"
                        type="password"
                        onChange={event => setUser({ ...user, password: event.target.value })} />
                    <Form.Label htmlFor="confirmedPassword">Подтвердите пароль</Form.Label>
                    <Form.Control
                        id="confirmedPassword"
                        type="password"
                        onChange={event => setConfirmedPassword(event.target.value)} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="success" onClick={ChangePassword }>Изменить</Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ButtonEditUserPassword;
