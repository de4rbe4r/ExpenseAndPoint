import { React, useState, useEffect } from 'react';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { AddCategoryUrl, EditCategoryUrl } from "../../Urls/UrlList";
import Button from 'react-bootstrap/Button';
import AlertModal from '../AlertModal';
import Cookies from 'universal-cookie';

const CategoryForm = ({ action, categoryToEdit, isShowForm, setIsShowForm, userId, setIsDataUpdated, isDataUpdated }) => {
    const [title, setTitle] = useState();
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");
    const [category, setCategory] = useState({
        id: '',
        userId: '',
        title: ''
    });
    const cookies = new Cookies();

    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };

    useEffect(() => {
        if (action === "Добавить") {
            setTitle("Добавление категории");
        } else if (action === "Изменить") {
            setTitle("Изменение категории");
        }

        if (categoryToEdit !== undefined) {
            setCategory({
                ...category,
                id: categoryToEdit.id,
                title: categoryToEdit.title,
                userId: categoryToEdit.userId
            });
        } else {
            setCategory({
                ...category,
                id: 0,
                userId: userId,
                title: ''
            });
        }
    }, [isShowForm]);

    const handleCloseForm = () => {
        setCategory({
            ...category,
            id: 0
        })
        setIsShowForm(false);
    }

    const AddEdtiCategory = (event) => {
        event.preventDefault();
        if (category.title === "") {
            setShowAlertModal(true);
            setErrorMessage("Введите название");
            setTitleAlert("Ошибка");
            return;
        }

        if (action === "Добавить") {
            axios.post(AddCategoryUrl, category, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Категория успешно добавлена");
                    setTitleAlert("Успешно");
                    setCategory({
                        ...category,
                        title: '',
                    });
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        } else if (action === "Изменить") {
            axios.put(EditCategoryUrl + category.id, category, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Категория успешно изменена");
                    setTitleAlert("Успешно");
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
            setIsShowForm(false);
        }
        setIsDataUpdated(!isDataUpdated);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }

    return (
        <>
        <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
        <Modal
        show={isShowForm}
        onHide={handleCloseForm}
        backdrop="static"
        keyboard={false}
    >
        <Modal.Header closeButton>
            <Modal.Title>{title}</Modal.Title>
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
            <Button variant="primary" onClick={AddEdtiCategory}>Добавить</Button>
        </Modal.Footer>
            </Modal>
        </>
    )
}

export default CategoryForm;