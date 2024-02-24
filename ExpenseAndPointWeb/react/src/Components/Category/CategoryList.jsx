import { React, useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import '../../App.css';
import Row from 'react-bootstrap/Row';
import useWindowDimensions from '../../Hooks/useWindowDimensions.jsx'
import axios from 'axios';
import { GetCategoryByIdUrl } from '../../Urls/UrlList';
import Cookies from 'universal-cookie';
import { ContextMenuTrigger, ContextMenu, ContextMenuItem } from 'rctx-contextmenu';
import ConfirmationForm from '../ConfirmationForm';
import CategoryForm from './CategoryForm';

const CategoryList = () => {
    const cookies = new Cookies();
    const [userId, setUserId] = useState(cookies.get('userId'));
    const { height, width } = useWindowDimensions();
    const [categoryList, setCategoryList] = useState(undefined);
    const [isDataUpdated, setDataUpdated] = useState(false);
    const [showEditForm, setShowEditForm] = useState(false);
    const [categoryToEdit, setCategoryToEdit] = useState();
    const [categoryToDelete, setCategoryToDelete] = useState();
    const [showConfirmationForm, setShowConfirmationForm] = useState(false);

    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };

    const setIsDataUpdated = (data) => {
        setDataUpdated(data);
    }

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId, config)
            .then(res => {
                setCategoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    setShowAlertModal(true);
                    setErrorMessage(error.message + ". " + error.response.data.detail);
                    setTitleAlert("Ошибка");
                }
            });
    }, [isDataUpdated]);

    const openEditForm = (id) => {
        setCategoryToEdit(categoryList.find(c => c.id === id));
        setShowEditForm(true);
    }
    
    const setIsShowEditForm = (data) => {
        setShowEditForm(data);
    }

    const openConfirmationForm = (id) => {
        setCategoryToDelete(categoryList.find(c => c.id === id));
        setShowConfirmationForm(true);
    }

    const setIsShowConfirmationForm = (data) => {
        setShowConfirmationForm(data);
    }

    if (categoryList === undefined) return <img src="../../loading.gif" style={{
        width: "100px",
    }}></img>;

    return (
        <>
            <CategoryForm action="Изменить" categoryToEdit={categoryToEdit}
                isShowForm={showEditForm} setIsShowForm={setIsShowEditForm} userId={userId} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
            <ConfirmationForm expense={undefined} category={categoryToDelete}
                isShowForm={showConfirmationForm} setIsShowForm={setIsShowConfirmationForm} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />

            <ListGroup className="overflow-auto"
            style={{
                height: 0.75 * height,
                overflow: scroll
                }}>
                <ListGroup.Item variant="dark" key='Title' value='Title'>
                    <Row className="justify-content-md-center">
                        Список категорий</Row>
                </ListGroup.Item>
                {
                    ((categoryList.count !== 0) ? (
                        categoryList.map((c, index) => (
                            <div key={`div-${c.id}`}>
                                <ContextMenuTrigger id={`menu-${c.id}`} key={`cmt-${c.id}`}>
                                    <ListGroup.Item action variant="dark" key={c.id} value={c.id}>
                                        <Row className="justify-content-md-center" value={c.id}>
                                            {c.title}
                                        </Row>
                                    </ListGroup.Item>
                                </ContextMenuTrigger>
                                <ContextMenu id={`menu-${c.id}`} key={`cm-${c.id}`}>
                                    <ContextMenuItem onClick={() => openEditForm(c.id)}>Изменить</ContextMenuItem>
                                    <ContextMenuItem onClick={() => openConfirmationForm(c.id)}>Удалить</ContextMenuItem>
                                </ContextMenu>
                            </div>
                        ))) : null)}
            </ListGroup>
        </>
    );
}

export default CategoryList;
