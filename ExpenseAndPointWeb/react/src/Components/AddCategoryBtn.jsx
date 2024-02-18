import { React, useState } from 'react';
import Cookies from 'universal-cookie';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { AddCategoryUrl } from "../Urls/UrlList";
import Button from 'react-bootstrap/Button';
import AlertModal from './AlertModal.jsx';



const AddCategoryBtn = ({ setIsDataUpdated, isDataUpdated }) => {
    const cookies = new Cookies();
    const [showCategory, setShowCategory] = useState(false);
    const handleCloseCategory = () => setShowCategory(false);
    const handleShowCategory = () => setShowCategory(true);
    const [category, setCategory] = useState({
        title: '',
        userId: cookies.get('userId'),
    });
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    const addCategory = (event) => {
        event.preventDefault();
        if (category.title === "") {
            setShowAlertModal(true);
            setErrorMessage("Введите название категории");
            setTitleAlert("Ошибка");
            return;
        }

        axios.post(AddCategoryUrl, category)
            .then(res => {
                setShowAlertModal(true);
                setErrorMessage("Категория с названием " + res.data.title + " успешно добавлена");
                setTitleAlert("Успешно");
                setCategory({ ...category, title: '' });
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
        setIsDataUpdated(!isDataUpdated);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    return (
        <>
        <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
        <Button variant="outline-light" size="lg" onClick={handleShowCategory}>Добавить категорию</Button>
        <Modal
        show={showCategory}
        onHide={handleCloseCategory}
        backdrop="static"
        keyboard={false}
    >
        <Modal.Header closeButton>
            <Modal.Title>Добавление категории</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            <Form.Label htmlFor="inputCategory">Название категории</Form.Label>
            <Form.Control
                type="text"
                id="inputCategory"
                value={category.title} onChange={event => setCategory({ ...category, title: event.target.value })}
            />
        </Modal.Body>
        <Modal.Footer>
            <Button variant="primary" onClick={addCategory}>Добавить</Button>
        </Modal.Footer>
            </Modal>
        </>
    )
}

export default AddCategoryBtn;